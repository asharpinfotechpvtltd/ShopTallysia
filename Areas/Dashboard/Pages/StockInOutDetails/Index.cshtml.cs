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
    public class IndexModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public IndexModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<StockInOut> StockInOut { get; set; } = default!;
        public string FilterType { get; set; } = "All"; // Default to "All"

        public async Task OnGetAsync(string filterType = "All")
        {
            FilterType = filterType;

            if (FilterType == "Sold")
            {
                StockInOut = await _context.TblStockInOut
                    .Include(s => s.Products)
                    .Include(s => s.TblWarehouseMaster)
                    .Where(s => s.Type == "Sold")
                    .ToListAsync();
            }
            else if (FilterType == "Added")
            {
                StockInOut = await _context.TblStockInOut
                    .Include(s => s.Products)
                    .Include(s => s.TblWarehouseMaster)
                    .Where(s => s.Type == "Added")
                    .ToListAsync();
            }
            else
            {
                StockInOut = await _context.TblStockInOut
                    .Include(s => s.Products)
                    .Include(s => s.TblWarehouseMaster)
                    .ToListAsync();
            }
        }
    }
}
