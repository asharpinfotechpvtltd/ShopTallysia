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
    public class EditModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public EditModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StockInOut StockInOut { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stockinout =  await _context.TblStockInOut.FirstOrDefaultAsync(m => m.LogId == id);
            if (stockinout == null)
            {
                return NotFound();
            }
            StockInOut = stockinout;
           ViewData["ProductId"] = new SelectList(_context.Set<Products>(), "Id", "Id");
           ViewData["WarehouseId"] = new SelectList(_context.TblWareHouse, "WarehouseId", "WarehouseId");
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

            _context.Attach(StockInOut).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockInOutExists(StockInOut.LogId))
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

        private bool StockInOutExists(int id)
        {
            return _context.TblStockInOut.Any(e => e.LogId == id);
        }
    }
}
