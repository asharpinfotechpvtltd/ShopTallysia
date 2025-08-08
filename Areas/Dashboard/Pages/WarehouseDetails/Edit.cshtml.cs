using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.WarehouseDetails
{
    public class EditModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public EditModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WareHouse WareHouse { get; set; } = default!;
       
        public SelectList StateNames { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse =  await _context.TblWareHouse.FirstOrDefaultAsync(m => m.WarehouseId == id);
            if (warehouse == null)
            {
                return NotFound();
            }
            WareHouse = warehouse;
            var stateList = await _context.TblIndianStates
       .Select(x => x.StateName)
       .ToListAsync();

            StateNames = new SelectList(stateList, WareHouse.Location);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string location)
        {
            WareHouse.Location = location;

            _context.Attach(WareHouse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WareHouseExists(WareHouse.WarehouseId))
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

        private bool WareHouseExists(int id)
        {
            return _context.TblWareHouse.Any(e => e.WarehouseId == id);
        }
    }
}
