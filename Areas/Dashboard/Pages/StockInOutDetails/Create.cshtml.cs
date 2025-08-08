using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.StockInOutDetails
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
            ViewData["ProductId"] = new SelectList(_context.Set<Products>(), "Id", "ProductName");
            ViewData["WarehouseId"] = new SelectList(_context.TblWareHouse, "WarehouseId", "WarehouseName");
            return Page();
        }

        [BindProperty]
        public StockInOut StockInOut { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {

            var product = await _context.TblProducts.Where(e => e.Id == StockInOut.ProductId).FirstOrDefaultAsync();

            StockInOut.SKU = product.skucode;
            // Check if the combination of ProductId and WarehouseId already exists in the Inventory table
            var inventoryItem = await _context.TblInventory
                .FirstOrDefaultAsync(i => i.ProductId == StockInOut.ProductId && i.WarehouseId == StockInOut.WarehouseId);

            // If the product and warehouse combination doesn't exist
            if (inventoryItem == null)
            {
                TempData["Message"] = "This combination of Product and Warehouse does not exist in the Inventory. Please create it first.";
                return Page();
            }

            // Check for "Sold" type and validate the quantity
            if (StockInOut.Type == "Sold")
            {
                if (StockInOut.Quantity > inventoryItem.TotalQuantity)
                {
                    TempData["Message"] = "The quantity to be sold exceeds the available stock.";
                    return Page();
                }
                else
                {
                    // Subtract from inventory if the type is "Sold"
                    inventoryItem.TotalQuantity -= StockInOut.Quantity;
                    _context.TblInventory.Update(inventoryItem);
                    TempData["Message"] = "Quantity has been sold and inventory updated.";
                }
            }
            else if (StockInOut.Type == "Added")
            {
                // Add to inventory if the type is "Added"
                inventoryItem.TotalQuantity += StockInOut.Quantity;
                _context.TblInventory.Update(inventoryItem);
                TempData["Message"] = "Quantity has been added to the inventory.";
            }

            // Add StockInOut log entry
            _context.TblStockInOut.Add(StockInOut);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
