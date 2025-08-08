using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceTelysia.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceTelysia.Pages
{
    public class AdminLoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AdminLoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AdminLogin Admin { get; set; }

        [TempData]
        public string LoginMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
           

            var adminUser = await _context.TblUsers
                .FirstOrDefaultAsync(a => a.Email == Admin.Email && a.Password == Admin.Password);

            if (adminUser != null)
            {
                var add = new UserLoggedInReports()
                {
                    LoggedInDate = DateTime.Now,
                    UserEmail = Admin.Email,
                };
             await   _context.TblUserLoggedInReports.AddAsync(add);
               await _context.SaveChangesAsync();
                HttpContext.Session.SetString("AdminEmail", adminUser.Email);
                HttpContext.Session.SetInt32("AdminId", adminUser.Id);
                return RedirectToPage("Index1", new { area = "Dashboard" });


            }
            else
            {
                
                LoginMessage = "Invalid email or password.";
                return Page();
            }
        }
    }
}
