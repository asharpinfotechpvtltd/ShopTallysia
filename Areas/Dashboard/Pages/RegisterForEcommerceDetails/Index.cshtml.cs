using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;
using EcommerceTelysia.Pages;

namespace EcommerceTelysia.Areas.Dashboard.Pages.RegisterForEcommerceDetails
{
    public class IndexModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public IndexModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<RegisterForEcommerce> RegisterForEcommerce { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RegisterForEcommerce = await _context.TblRegisterForEcommerce.ToListAsync();
        }
    }
}
