using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.CounponCodesDetails
{
    public class DeleteModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DeleteModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CouponCodes CouponCodes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var couponcodes = await _context.TblCouponCodes.FirstOrDefaultAsync(m => m.Id == id);

            if (couponcodes is not null)
            {
                CouponCodes = couponcodes;

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

            var couponcodes = await _context.TblCouponCodes.FindAsync(id);
            if (couponcodes != null)
            {
                CouponCodes = couponcodes;
                _context.TblCouponCodes.Remove(CouponCodes);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
