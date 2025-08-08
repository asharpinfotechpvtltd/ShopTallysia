using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages
{
    public class Index1Model : PageModel
    {
        private readonly ApplicationDbContext _context;
        public Index1Model(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public int Packages { get; set; }= 0;
        [BindProperty]
        public int Blogs { get; set; } = 0;
       
        public void OnGet()
        {
           
           
        }
    }
}
