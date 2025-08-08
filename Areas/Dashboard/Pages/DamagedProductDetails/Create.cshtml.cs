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
        public DamagedProducts DamagedProducts { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var product = await _context.TblProducts.Where(e => e.Id == DamagedProducts.ProductId).FirstOrDefaultAsync();
            DamagedProducts.ProductName = product.ProductName;
            DamagedProducts.SKU = product.skucode;

            _context.TblDamagedProducts.Add(DamagedProducts);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
