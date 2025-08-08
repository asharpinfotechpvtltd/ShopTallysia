using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.DamagedProductDetails
{
    public class DetailsModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DetailsModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public DamagedProducts DamagedProducts { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var damagedproducts = await _context.TblDamagedProducts.FirstOrDefaultAsync(m => m.DamageReportId == id);

            if (damagedproducts is not null)
            {
                DamagedProducts = damagedproducts;

                return Page();
            }

            return NotFound();
        }
    }
}
