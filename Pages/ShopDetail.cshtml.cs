using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.@class;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Pages
{
    public class ShopDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly AddToCartItems _item;
        public ShopDetailModel(ApplicationDbContext context, AddToCartItems items)
        {
            _context = context;
            _item = items;
        }
        [BindProperty]
        public Products Products { get; set; }
        [BindProperty]
        public IList<Products> Products2 { get; set; }
        [BindProperty]
        public Reviews Reviews { get; set; } = new Reviews();
        [BindProperty]
        public int TotalReviews { get; set; }
        [BindProperty]
        public int AverageReviews { get; set; }
        [BindProperty]
        public string productname { get; set; }
        [BindProperty]
        public string categoryname { get; set; }
      

        public async Task OnGet(string productid, string productname)
        {

            HttpContext.Session.SetString("ProductId", productid);

            Products = await _context.TblProducts.Where(e => e.skucode == productid).FirstOrDefaultAsync();
          
          
            Reviews = await _context.TblReviews
       .FirstOrDefaultAsync(e => e.ProductId == productid && e.Isviewable == true)
       ?? new Reviews();
            TotalReviews = await _context.TblReviews.Where(e => e.ProductId == productid).CountAsync();
            var productReviews = await _context.TblReviews
      .Where(e => e.ProductId == productid)
      .Select(e => e.review)
      .ToListAsync();

            double averageReviews = productReviews.Any() ? productReviews.Average() : 0;
            AverageReviews = (int)Math.Round(averageReviews);


        }
        public async Task<IActionResult> OnPostReview(string email, string name, string review, int star)
        {
            string pid = HttpContext.Session.GetString("ProductId");

            var reviews = new Reviews
            {
               
                email = email,
                review = star,
                Date = DateTime.Now,
                comment = review,
                ProductId = pid,
                Isviewable = false
            };

            _context.TblReviews.Add(reviews);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostAddToCart(int quantity, bool isBuyNow = false)
        {
            try
            {
                var useremail = HttpContext.Session.GetString("UserEmail");
                var username = HttpContext.Session.GetString("Name");
                var phone = HttpContext.Session.GetString("Mobile");
                string productid = HttpContext.Session.GetString("ProductId");


                if (phone == null && isBuyNow == false)
                {
                    string redirectUrl = Url.Page("Index");
                    return RedirectToPage("/Register", new { redirectUrl });

                }

                await _item.OnPostAddToCarts(quantity, productid, isBuyNow,  phone, username);


                if (isBuyNow == true)
                {

                    return RedirectToPage("/checkout", new { isBuyNow = true });
                }


                return RedirectToPage("cart");


            }
            catch (Exception ex)
            {
                return Page();
            }
        }

    }
}
