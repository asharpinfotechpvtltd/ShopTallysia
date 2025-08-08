using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.ProductDetails
{
    public class DetailsModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DetailsModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public Products Products { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.TblProducts.FirstOrDefaultAsync(m => m.Id == id);

            if (products is not null)
            {
                Products = products;

                return Page();
            }

            return NotFound();
        }
    }
}
