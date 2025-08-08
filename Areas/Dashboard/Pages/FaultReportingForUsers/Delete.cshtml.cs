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
    public class DeleteModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public DeleteModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faultrepotingforusers = await _context.TblFaultRepotingForUsers.FindAsync(id);
            if (faultrepotingforusers != null)
            {
                FaultRepotingForUsers = faultrepotingforusers;
                _context.TblFaultRepotingForUsers.Remove(FaultRepotingForUsers);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
