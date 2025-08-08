using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.StockInOutDetails
{
    public class DetailsModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DetailsModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public StockInOut StockInOut { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockinout = await _context.TblStockInOut.FirstOrDefaultAsync(m => m.LogId == id);

            if (stockinout is not null)
            {
                StockInOut = stockinout;

                return Page();
            }

            return NotFound();
        }
    }
}
