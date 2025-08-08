using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EcommerceTelysia.Areas.Dashboard.Pages.Inventory_Details
{
    public class IndexModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public IndexModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<GroupedInventoryViewModel> GroupedInventory { get; set; } = new();
        public bool ShowOutOfStock { get; set; }

        public async Task OnGetAsync(bool showOutOfStock = false)
        {
            ShowOutOfStock = showOutOfStock;

            var groupedQuery = _context.TblInventory
                .Include(i => i.Product)
                .Include(i => i.TblWarehouseMaster)
                .GroupBy(i => i.ProductId)
                .Select(g => new GroupedInventoryViewModel
                {
                    ProductId = g.Key,
                    ProductName = g.Select(x => x.Product.ProductName).FirstOrDefault(),
                    SKU = g.Select(x => x.SKU).FirstOrDefault(),
                    TotalQuantity = g.Sum(x => x.TotalQuantity),
                    LastUpdated = g.Max(x => x.LastUpdated)
                });

            if (ShowOutOfStock)
            {
                groupedQuery = groupedQuery.Where(g => g.TotalQuantity <= 5);
            }

            GroupedInventory = await groupedQuery.ToListAsync();
        }
        public async Task<IActionResult> OnPost(int id)
        {
            var inventories = await _context.TblInventory
                .Where(i => i.ProductId == id)
                .ToListAsync();

            if (inventories == null || inventories.Count == 0)
            {
                return NotFound();
            }

            _context.TblInventory.RemoveRange(inventories);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public class GroupedInventoryViewModel
        {
            public int ProductId { get; set; }
            public string? ProductName { get; set; }
            public string? SKU { get; set; }
            public int TotalQuantity { get; set; }
            public DateTime? LastUpdated { get; set; }
        }
    }
}