using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.ProductsReviewDetails
{
    public class DeleteModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DeleteModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reviews Reviews { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.TblReviews.FirstOrDefaultAsync(m => m.Id == id);

            if (reviews is not null)
            {
                Reviews = reviews;

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

            var reviews = await _context.TblReviews.FindAsync(id);
            if (reviews != null)
            {
                Reviews = reviews;
                _context.TblReviews.Remove(Reviews);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
