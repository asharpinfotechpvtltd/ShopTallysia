using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.Inventory_Details
{
    public class DeleteModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DeleteModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inventory Inventory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.TblInventory.FirstOrDefaultAsync(m => m.InventoryId == id);

            if (inventory is not null)
            {
                Inventory = inventory;

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

            var inventory = await _context.TblInventory.Where(e=>e.ProductId==id).FirstOrDefaultAsync();
            var stockinout = await _context.TblStockInOut.Where(e => e.WarehouseId == inventory.WarehouseId && e.ProductId == inventory.ProductId).ToListAsync();
            if (stockinout != null)
            {
                _context.TblStockInOut.RemoveRange(stockinout);
            }
                if (inventory != null)
            {
                Inventory = inventory;
                _context.TblInventory.RemoveRange(Inventory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
