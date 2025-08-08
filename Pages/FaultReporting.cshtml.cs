using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Pages
{
    public class FaultReportingModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public FaultReportingModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string Name, string Email, string Address, string Number,string Description)
        {
            var add = new FaultRepotingForUsers
            {
                Name = Name ?? "NA",
                email = Email ?? "NA",
                Number = Number ?? "NA",
                Address = Address ?? "NA",
                Description=Description??"NA",
                Status="Open",
                CreatedAt = DateTime.Now,



            };
            await _context.TblFaultRepotingForUsers.AddAsync(add);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");

        }
    }
}
