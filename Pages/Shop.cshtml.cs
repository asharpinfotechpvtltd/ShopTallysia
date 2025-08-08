using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Pages
{
    public class ShopModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ShopModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public IList<Products> Products { get; set; }
        public async Task OnGet()
        {
            Products = await _context.TblProducts.Where(e=>e.Visible==true).ToListAsync();
        }
    }
}
