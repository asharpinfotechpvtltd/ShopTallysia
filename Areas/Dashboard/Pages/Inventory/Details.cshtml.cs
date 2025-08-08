using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.Inventory_Details
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string? ProductName { get; set; }

        public List<WarehouseQuantityInfo> WarehouseQuantities { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _context.TblProducts.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound();

            ProductName = product.ProductName;

            WarehouseQuantities = await _context.TblInventory
                .Include(i => i.TblWarehouseMaster)
                .Where(i => i.ProductId == id)
                .Select(i => new WarehouseQuantityInfo
                {
                    WarehouseName = i.TblWarehouseMaster.WarehouseName,
                    Quantity = i.TotalQuantity
                })
                .ToListAsync();

            return Page();
        }

        public class WarehouseQuantityInfo
        {
            public string WarehouseName { get; set; } = "";
            public int Quantity { get; set; }
        }
    }
}
