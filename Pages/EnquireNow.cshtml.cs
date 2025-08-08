using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Pages
{
    public class EnquireNowModel : PageModel
    {private readonly ApplicationDbContext _context;
        public EnquireNowModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost(string Name,string Email,string Address,string Number)
        {
            var add = new Enquiry
            {
                Name = Name ?? "NA",
                email = Email ?? "NA",
                Number = Number ?? "NA",
                Address = Address ?? "NA",
                CreatedAt=DateTime.Now,

                

            };
            await _context.TblEnquiry.AddAsync(add);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");

        }
    }
}
