using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.CounponCodesDetails
{
    public class EditModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public EditModel(EcommerceTelysia.Models.ApplicationDbContext context)
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

            var couponcodes =  await _context.TblCouponCodes.FirstOrDefaultAsync(m => m.Id == id);
            if (couponcodes == null)
            {
                return NotFound();
            }
            CouponCodes = couponcodes;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CouponCodes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponCodesExists(CouponCodes.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CouponCodesExists(int id)
        {
            return _context.TblCouponCodes.Any(e => e.Id == id);
        }
    }
}
