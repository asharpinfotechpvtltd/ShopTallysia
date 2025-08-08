using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.CartMarketingDetails
{
    public class DeleteModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DeleteModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cart Cart { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.TblCart.FirstOrDefaultAsync(m => m.Id == id);

            if (cart is not null)
            {
                Cart = cart;

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

            var cart = await _context.TblCart.FindAsync(id);
            if (cart != null)
            {
                Cart = cart;
                _context.TblCart.Remove(Cart);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
