using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.ProductsReviewDetails
{
    public class CreateModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public CreateModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Reviews Reviews { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TblReviews.Add(Reviews);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
