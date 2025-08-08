using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EcommerceTelysia.Models;
using static System.Net.Mime.MediaTypeNames;

namespace EcommerceTelysia.Areas.Dashboard.Pages.ProductDetails
{
    public class CreateModel : PageModel
    {
        private readonly EcommerceTelysia.Models.ApplicationDbContext _context;

        private readonly IWebHostEnvironment _environment;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Products Products { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile Image)
        {
            string profileFolder = Path.Combine(_environment.WebRootPath, "products");
            Directory.CreateDirectory(profileFolder);

            string fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
            string filePath1 = Path.Combine(profileFolder, fileName1);

            using (var stream = new FileStream(filePath1, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }

            Products.Image = $"products/{fileName1}";

            _context.TblProducts.Add(Products);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
