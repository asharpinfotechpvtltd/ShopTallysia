using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.Inventory_Details
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inventory Inventory { get; set; }

        [BindProperty]
        public List<string> SelectedWarehouseIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Inventory = await _context.TblInventory.FindAsync(id);
            if (Inventory == null) return NotFound();

            ViewData["ProductId"] = new SelectList(_context.TblProducts, "ProductId", "ProductName");
            ViewData["WarehouseId"] = new SelectList(_context.TblWareHouse, "WarehouseId", "WarehouseName");


            // Example: Fetch existing warehouse links (assuming another table)
            SelectedWarehouseIds = await _context.TblInventory
                .Where(iw => iw.InventoryId == Inventory.InventoryId)
                .Select(iw => iw.WarehouseId.ToString())
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Attach(Inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Remove old mappings
                var oldMappings = _context.TblInventory.Where(iw => iw.InventoryId == Inventory.InventoryId);
                _context.TblInventory.RemoveRange(oldMappings);
                await _context.SaveChangesAsync();

                // Add new mappings
                foreach (var warehouseId in SelectedWarehouseIds)
                {
                    _context.TblInventory.Add(new Inventory
                    {
                        InventoryId = Inventory.InventoryId,
                        WarehouseId = int.Parse(warehouseId)
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(Inventory.InventoryId)) return NotFound();
                else throw;
            }

            return RedirectToPage("./Index");
        }

        private bool InventoryExists(int id) =>
            _context.TblInventory.Any(e => e.InventoryId == id);
    }

}
