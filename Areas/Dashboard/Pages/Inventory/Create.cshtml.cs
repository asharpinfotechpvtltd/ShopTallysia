using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.Inventory_Details
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int SelectedProductId { get; set; }

        [BindProperty]
        public List<Inventory> Inventories { get; set; } = new();

        public List<SelectListItem> AvailableProducts { get; set; } = new();
        public List<SelectListItem> WarehouseList { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get product IDs already in inventory
            var existingProductIds = await _context.TblInventory
                .Select(i => i.ProductId)
                .Distinct()
                .ToListAsync();

            // Get only products that are not in inventory
            AvailableProducts = await _context.TblProducts
                .Where(p => !existingProductIds.Contains(p.Id))
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.ProductName
                }).ToListAsync();

            WarehouseList = await _context.TblWareHouse
                .Select(w => new SelectListItem
                {
                    Value = w.WarehouseId.ToString(),
                    Text = w.WarehouseName
                }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            int SelectedProductId,
            List<int> warehouseIds,
            List<int> quantities)
        {
            if (warehouseIds == null || quantities == null || warehouseIds.Count != quantities.Count)
            {
                ModelState.AddModelError("", "Invalid warehouse or quantity data.");
                await OnGetAsync(); // reload lists
                return Page();
            }

            var product = await _context.TblProducts.FirstOrDefaultAsync(p => p.Id == SelectedProductId);
            if (product == null)
            {
                ModelState.AddModelError("", "Invalid product.");
                await OnGetAsync(); // reload lists
                return Page();
            }

            for (int i = 0; i < warehouseIds.Count; i++)
            {
                var inventory = new Inventory
                {
                    ProductId = SelectedProductId,
                    ProductName = product.ProductName,
                    SKU = product.skucode,
                    WarehouseId = warehouseIds[i],
                    TotalQuantity = quantities[i],
                    LastUpdated = DateTime.Now
                };

                _context.TblInventory.Add(inventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

    }
}
