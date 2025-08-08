using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.DamagedProductDetails
{
    public class EditModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public EditModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DamagedProducts DamagedProducts { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var damagedproducts =  await _context.TblDamagedProducts.FirstOrDefaultAsync(m => m.DamageReportId == id);
            if (damagedproducts == null)
            {
                return NotFound();
            }
            DamagedProducts = damagedproducts;
           ViewData["ProductId"] = new SelectList(_context.Set<Products>(), "Id", "ProductName");
           ViewData["WarehouseId"] = new SelectList(_context.TblWareHouse, "WarehouseId", "WarehouseName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var product = await _context.TblProducts.Where(e => e.Id == DamagedProducts.ProductId).FirstOrDefaultAsync();
            DamagedProducts.ProductName = product.ProductName;
            DamagedProducts.SKU = product.skucode;
            _context.Attach(DamagedProducts).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DamagedProductsExists(DamagedProducts.DamageReportId))
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

        private bool DamagedProductsExists(int id)
        {
            return _context.TblDamagedProducts.Any(e => e.DamageReportId == id);
        }
    }
}
