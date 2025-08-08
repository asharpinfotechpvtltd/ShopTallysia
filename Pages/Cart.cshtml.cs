using Astaberry.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Class;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Pages
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CartModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public CouponCodes Coupon { get; set; }
        public string CouponMessage { get; set; }
        public List<Item> Carts { get; set; } = new List<Item>();

        public List<Cart> CartMarketing { get; set; } = new List<Cart>();

        public double Subtotal { get; set; }
        public double Shipping { get; set; }
        public double Total { get; set; }
        public double CouponDiscount { get; set; }
        public double DiscountAmount { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var productID = HttpContext.Session.GetString("productid");
                var quantity = HttpContext.Session.GetInt32("quantity");

                // Get the current URL

                var Mobile = HttpContext.Session.GetString("Mobile");
                Carts = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") ?? new List<Item>();

                if (Mobile != null && Carts.Count == 0)
                {
                    CartMarketing = await _context.TblCart.Where(e => e.Phone == Mobile).ToListAsync();
                    if (CartMarketing.Count > 0 && CartMarketing.Any())
                    {
                        Carts = CartMarketing.Select(cart => new Item
                        {
                            ProductName = cart.ProductName,
                            ProductId = cart.ProductId,
                            Qty = cart.Qty,

                            Price = Convert.ToDouble(cart.Price),

                            Image = cart.Image
                        }).ToList();

                        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Carts);
                    }
                    else
                    {
                        Carts = new List<Item>();
                    }

                }

                // Retrieve the cart from the session
                else

                {
                    CartMarketing = await _context.TblCart.Where(e => e.Phone == Mobile).ToListAsync();
                    if (CartMarketing.Count > 0 && CartMarketing.Any())
                    {
                        Carts = CartMarketing.Select(cart => new Item
                        {
                            ProductName = cart.ProductName,
                            ProductId = cart.ProductId,
                            Qty = cart.Qty,

                            Price = Convert.ToDouble(cart.Price),

                            Image = cart.Image
                        }).ToList();

                        SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Carts);
                    }
                    else
                    {
                        Carts = new List<Item>();
                    }
                }
             

                // Calculate totals
                CalculateTotals();

                // Update the cart in the session
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Carts);
                return Page();
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is implemented)
                return Page();
            }
        }

        private void CalculateTotals()
        {
            string check = HttpContext.Session.GetString("AppliedCoupon");
            var discountvalue = HttpContext.Session.GetInt32("Discount");
            Total = 0;


            foreach (var item in Carts)
            {
                if (item == null) continue;

                Total += item.Price * item.Qty;

            }

            if (Total < 3000)
            {
                Shipping = 80;
                Total += Shipping;
            }
            else
            {
                Shipping = 0;
            }

            Subtotal = Total;
            if (check == "True")
            {

                DiscountAmount = Subtotal * (double)discountvalue / 100;
                Subtotal -= DiscountAmount;
                HttpContext.Session.SetString("AppliedCoupon", "True");
            }
        }
        public async Task<IActionResult> OnPostCouponCode(string coupon_code)
        {
            var carts = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") ?? new List<Item>();

            if (carts == null || !carts.Any())
            {
                TempData["CouponMessage"] = "Your cart is empty. Add items to the cart before applying a coupon.";
                return RedirectToPage("cart");
            }

            Coupon = await _context.TblCouponCodes.FirstOrDefaultAsync(e => e.CouponCode == coupon_code);

            if (Coupon == null)
            {
                TempData["CouponMessage"] = "Invalid coupon code.";
            }
            else if (!Coupon.IsActive)
            {
                TempData["CouponMessage"] = "This coupon is no longer active.";
            }
            else if (DateTime.Now < Coupon.ApplicableFrom || DateTime.Now > Coupon.ApplicableTo)
            {
                TempData["CouponMessage"] = "This coupon is not valid for the selected date.";
            }
            else
            {
                // Set coupon details in session
                HttpContext.Session.SetString("AppliedCoupon", "True");
                HttpContext.Session.SetInt32("Discount", Coupon.DiscountPercentage);

                // Recalculate totals
                CalculateTotals();

                TempData["CouponMessage"] = "Coupon applied successfully! Discount has been applied to your total.";
            }

            return RedirectToPage("cart");
        }


        public async Task<IActionResult> OnPostUpdateQuantity(string productID, int quantity, string size)
        {
            try
            {
                // Get the cart from the session
                var Usercart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

                if (Usercart == null || !Usercart.Any())
                {
                    return BadRequest("Cart is empty.");
                }

                // Find the item in the session cart using ProductID and size
                var cartItem = Usercart.FirstOrDefault(i => i.ProductId == productID);

                if (cartItem != null)
                {
                    // Update the quantity in the session cart
                    cartItem.Qty = quantity;

                    // Update the cart in the session
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Usercart);

                    // Get the user's email from the session
                    var mobile= HttpContext.Session.GetString("Mobile");

                    // Find the corresponding item in the TblCarts table
                    var dbCartItem = await _context.TblCart.FirstOrDefaultAsync(c =>
                        c.Phone == mobile &&
                        c.ProductId == productID
                        );

                    if (dbCartItem != null)
                    {
                        // Update the quantity in the database
                        dbCartItem.Qty = quantity;

                        // Save changes to the database
                        await _context.SaveChangesAsync();
                    }

                    // Redirect to the Cart page to prevent form re-submission issues
                    return RedirectToPage("Cart");
                }
                else
                {
                    return BadRequest("Item not found in the cart.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is implemented)
                return Page();
            }
        }


        public async Task<IActionResult> OnGetDelete(string id, string size, string addon, string material)
        {
            try
            {
                // Retrieve the cart from the session
                Carts = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

                if (Carts == null || !Carts.Any())
                {
                    return BadRequest("Cart is empty or not found.");
                }

                // Retrieve the user email from session
                var mobile = HttpContext.Session.GetString("Mobile");

                // Remove the item from the database if it exists
                var itemInDb = await _context.TblCart.FirstOrDefaultAsync(e =>
                    e.Phone == mobile &&

                    e.ProductId == id);

                if (itemInDb != null)
                {
                    _context.TblCart.Remove(itemInDb);
                    await _context.SaveChangesAsync();
                }

                // Find the item in the session cart
                var itemInSession = Carts.FirstOrDefault(c =>
                    c.ProductId == id
                  );

                if (itemInSession != null)
                {
                    Carts.Remove(itemInSession);
                    // Update the session cart
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", Carts);
                }

                // Redirect back to the Cart page
                return RedirectToPage("Cart");
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is implemented)
                return Page();
            }
        }
    }

}
