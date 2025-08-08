using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Models;

namespace EcommerceTelysia.Areas.Dashboard.Pages.ProductsReviewDetails
{
    public class IndexModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        public IndexModel(EcommerceTelysia.Models.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Reviews> Reviews { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Reviews = await _context.TblReviews.ToListAsync();
        }
    }
}
