using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.FaultReportingForUsers
{
    public class DetailsModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DetailsModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public FaultRepotingForUsers FaultRepotingForUsers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultrepotingforusers = await _context.TblFaultRepotingForUsers.FirstOrDefaultAsync(m => m.TicketId == id);

            if (faultrepotingforusers is not null)
            {
                FaultRepotingForUsers = faultrepotingforusers;

                return Page();
            }

            return NotFound();
        }
    }
}
