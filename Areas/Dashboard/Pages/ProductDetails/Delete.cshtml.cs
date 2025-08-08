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
    public class DeleteModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DeleteModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.TblProducts.FindAsync(id);
            if (products != null)
            {
                Products = products;
                _context.TblProducts.Remove(Products);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
