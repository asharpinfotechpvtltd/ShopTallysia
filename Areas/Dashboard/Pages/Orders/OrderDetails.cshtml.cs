using EcommerceTelysia.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace EcommerceTelysia.Areas.Dashboard.Pages.Orders
{
    public class OrderDetailsModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;
        public OrderDetailsModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }
        public TblShippingDetail ShippingDetail { get; set; }
        [BindProperty]
        public TblBillingDetail BillingDetail { get; set; }
        public TblCustomerOrderDetails CustomerOrderDetails { get; set; }
        [BindProperty]
        public List<CustomerdetailsDTO> CustomerdetailsDTOs { get; set; }
        public List<Products> Products { get; set; }
        public TblOrderId OrderId { get; set; }
        public bool DetailsNotFound { get; set; }

        public class CustomerdetailsDTO
        {
            public int Id { get; set; }
            public string OrderCode { get; set; }
            public string SkuCode { get; set; }
            public int Qty { get; set; }
            public double Price { get; set; }
            public string Status { get; set; }
            public double Gst { get; set; }
            public string Material { get; set; }
            public string Size { get; set; }
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string AddOn { get; set; }

        }

        public decimal SubTotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Total { get; set; }
        public decimal CouponDiscount { get; set; } = 0.05m;  // Changed to decimal with 'm' suffix
        public decimal DiscountAmount { get; set; }
        public decimal AfterDiscount { get; set; }



        public async Task<IActionResult> OnGetAsync(string orderid)
        {
            string check = HttpContext.Session.GetString("AppliedCoupon");
            var discountvalue = HttpContext.Session.GetInt32("Discount");
            if (string.IsNullOrEmpty(orderid))
            {
                return RedirectToPage("./BillingDetails");
            }

            ShippingDetail = await _context.TblShippingDetail
                .FirstOrDefaultAsync(s => s.Orderid == orderid);

            BillingDetail = await _context.TblBillingDetail
                .FirstOrDefaultAsync(b => b.Orderid == orderid);
      
            CustomerdetailsDTOs = await (from c in _context.TblCustomerOrderDetails
                                         join p in _context.TblProducts
                                         on new { c.ProductId } equals new { ProductId = p.skucode }
                                         where c.OrderCode == orderid
                                         select new CustomerdetailsDTO
                                         {
                                             Id = c.Id,
                                             OrderCode = c.OrderCode,

                                             Qty = c.Qty,
                                             Price = c.Price,
                                             Status = c.Status,
                                             Gst = c.Gst,

                                             ProductId = c.ProductId,
                                             ProductName=p.ProductName
                                             

                                             // Adding ProductName from Product table
                                         }).ToListAsync();


            if (CustomerdetailsDTOs != null && CustomerdetailsDTOs.Any())
            {
                SubTotal = (decimal)CustomerdetailsDTOs.Sum(i => (double)(i.Price * i.Qty));
                if (check == "True")
                {
                    DiscountAmount = SubTotal * (decimal)discountvalue / 100;
                    // Calculate AfterDiscount
                    AfterDiscount = SubTotal - DiscountAmount;

                    // Apply shipping if the discounted subtotal is less than 3000
                    Shipping = AfterDiscount < 3000 ? 80 : 0;

                    // Ensure Shipping is also a decimal for consistency
                    Total = AfterDiscount + (decimal)Shipping;

                }
                else
                {
                    Shipping = SubTotal < 3000 ? 80 : 0;

                    // Ensure Shipping is also a decimal for consistency
                    Total = SubTotal + (decimal)Shipping;
                }



            }


            if (ShippingDetail == null && BillingDetail == null && CustomerOrderDetails == null)
            {
                DetailsNotFound = true; ;
            }
            OrderId = await _context.TblOrderId.FirstOrDefaultAsync(e => e.Orderid == orderid);

            return Page();
        }
        public async Task<IActionResult> OnPostUpdateAsync(string orderid, string Status)
        {
            // Retrieve the Order by OrderId
            var order = await _context.TblOrderId.FirstOrDefaultAsync(e => e.Orderid == orderid);


            // Update status in the OrderId table
            order.Status = Status;

            // Update status in the CustomerOrderDetails table for matching OrderId
            CustomerdetailsDTOs = await _context.TblCustomerOrderDetails
                                       .Where(c => c.OrderCode == orderid)
                                       .Select(c => new CustomerdetailsDTO
                                       {
                                           Id = c.Id,
                                           OrderCode = c.OrderCode,

                                           Qty = c.Qty,
                                           Price = c.Price,
                                           Status = Status,  
                                           Gst = c.Gst,

                                           ProductId = c.ProductId
                                       })
                                       .ToListAsync();

          
            foreach (var dto in CustomerdetailsDTOs)
            {
                var detail = await _context.TblCustomerOrderDetails.FindAsync(dto.Id);
                if (detail != null)
                {
                    detail.Status = dto.Status;
                }
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
            return RedirectToPage(); // Redirect to the same page to show updated status
        }

        public async Task<IActionResult> OnPostUpdateStatus(string SkuCode, string Status, string orderId)
        {
            // Retrieve the required properties using Select with explicit double casting
            var customerOrderDetail = await _context.TblCustomerOrderDetails
                .Where(e => e.ProductId == SkuCode && e.OrderCode == orderId).FirstOrDefaultAsync();





            customerOrderDetail.Status = Status;

            _context.Update(customerOrderDetail);
            await _context.SaveChangesAsync();


            return RedirectToPage("OrderDetails");
        }



    }
}
