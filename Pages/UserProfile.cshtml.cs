using EcommerceTelysia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EcommerceTelysia.Pages
{
    public class UserProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public UserProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public TblOrderId Orders { get; set; } = new TblOrderId();
        public IList<TblCustomerOrderDetails> CustomerOrderDetails { get; set; } = new List<TblCustomerOrderDetails>();

        public async Task<IActionResult> OnGet()
        {
            string mobile = HttpContext.Session.GetString("Mobile");
            if (mobile == null)
            {
                return RedirectToPage("Register");
            }

            Orders = await _context.TblOrderId
                .Where(e => e.Mobile == mobile)
                .OrderByDescending(o => o.Date) 
                .FirstOrDefaultAsync();

            if (Orders != null)
            {
                CustomerOrderDetails = await _context.TblCustomerOrderDetails
                    .Where(e => e.OrderCode == Orders.Orderid)
                    .ToListAsync();
            }

            return Page();
        }
    }
}