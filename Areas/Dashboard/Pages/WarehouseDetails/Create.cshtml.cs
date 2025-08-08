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
    public class CreateModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public CreateModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }
      
        public SelectList StateNames { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            var states = await _context.TblIndianStates.ToListAsync();
            StateNames = new SelectList(states, "StateName", "StateName");
            return Page();
        }

        [BindProperty]
        public WareHouse WareHouse { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string location)
        {
            WareHouse.Location = location;

            _context.TblWareHouse.Add(WareHouse);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
