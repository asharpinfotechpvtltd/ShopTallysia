using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Pages
{
    public class myaccounttestModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public myaccounttestModel(ApplicationDbContext context)
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
    
    public async Task<IActionResult> OnPostLogin()
        {
            return RedirectToPage("SolarCalculator");
        }
    } }