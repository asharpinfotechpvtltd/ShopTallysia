using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
 // your DbContext namespace
using EcommerceTelysia.Models; // your model namespace
using System.Linq;
using EcommerceTelysia.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using EcommerceTelysia.Class;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EcommerceTelysia.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string State { get; set; }

        [BindProperty]
        public string Mobile { get; set; }

        [BindProperty]
        public string Email { get; set; }
        public SelectList StateNames { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            var states = await _context.TblIndianStates.ToListAsync();
            StateNames = new SelectList(states, "StateName", "StateName");
            var check = HttpContext.Session.GetString("Mobile");
            if (check != null)
            {
                return RedirectToPage("shop");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostRegister(string Name, string Mobile, string Email, string Password)
        {
            try
            {
                
                var exists = await _context.TblRegisterForEcommerce.AnyAsync(r => r.Mobile == Mobile);
                if (exists)
                {
                    TempData["FailureMessage"] = "Registration failed! Mobile number already exists.";
                    ModelState.AddModelError(string.Empty, "This mobile number is already registered.");
                    return Page();
                }

             
                var regNoParam = new SqlParameter
                {
                    ParameterName = "@RegNo",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 15,
                    Direction = ParameterDirection.Output
                };

               
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC SpRegisterForEcommerce @Name, @Mobile, @Email, @Password, @RegNo OUT",
                    new SqlParameter("@Name", Name),
                    new SqlParameter("@Mobile", Mobile),
                    new SqlParameter("@Email", Email ?? "NA"),
                    new SqlParameter("@Password", Password),
                    regNoParam
                );

                // Get generated Registration Number
                string regNo = regNoParam.Value?.ToString() ?? "REG00000";

                // Store in session
                HttpContext.Session.SetString("Mobile", Mobile);
                HttpContext.Session.SetString("Email", Email ?? "NA");
                HttpContext.Session.SetString("Name", Name);
                HttpContext.Session.SetString("RegistrationNumber", regNo);

                // Generate and send OTP
                string otp = await GenerateOtp();
                HttpContext.Session.SetString("Otp", otp);

                await SendOtpSmsAsync(Mobile, otp);

                return RedirectToPage("OtpVerify");
            }
            catch (Exception ex)
            {
                TempData["FailureMessage"] = "Something went wrong!";
                ModelState.AddModelError(string.Empty, "Something went wrong while processing your request.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostLogin(string LoginEmail,string LoginPassword)
        {
            var check = await _context.TblRegisterForEcommerce.Where(e => e.Email == LoginEmail && e.Password == LoginPassword).FirstOrDefaultAsync();
            if (check != null)
            {
                TempData["SuccessMessage"] = "Login successful!";
                HttpContext.Session.SetString("Mobile", check.Mobile);
                HttpContext.Session.SetString("Email", check.Email ?? "NA");
                HttpContext.Session.SetString("Name", check.Name);
                HttpContext.Session.SetString("RegistrationNumber", check.RegistrationNumber);
                return RedirectToPage("shop");

                string otp = await GenerateOtp();
                if (!string.IsNullOrEmpty(check.Mobile))
                {
                    await SendOtpSmsAsync(check.Mobile, otp);
                }
                return RedirectToPage("OtpVerify");

            }

            TempData["FailureMessage"] = "Login failed!";
            return Page();
        }
        private async Task<string> GenerateOtp()
        {
            int otpInt;
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                otpInt = Math.Abs(BitConverter.ToInt32(randomNumber, 0)) % 1000000;
            }

            string otpStr = otpInt.ToString("D6");

            var add = new OTPVerification
            {
                Number = otpInt,
                MobileNo = HttpContext.Session.GetString("Mobile"),
                Date=DateTime.Now,
             
            };
            await _context.TblOTPVerification.AddAsync(add);
            await _context.SaveChangesAsync();

            return otpStr;
        }

        private async Task<IActionResult>SendOtpSmsAsync(string mobile, string otp)
        {
            try
            {
                string smsApiUrl = SMSCredentials.smsApiUrl;
                string user = SMSCredentials.user; 
                string password = SMSCredentials.password; 
                string senderid = SMSCredentials.senderid;
                string channel = SMSCredentials.channel;
                string DCS =SMSCredentials.DCS;
                string flashsms = SMSCredentials.flashsms;
                string number = "91" + mobile; 
                string expiry = SMSCredentials.expiry; 
                string text = $"Your OTP for TALLYSIA ENERGIES Registration for mobile verification is {otp}. This will expire after {expiry}. From TALLYSIAENERGIES.";
                string route = SMSCredentials.route;
                string peid = SMSCredentials.peid;
                string DLTTemplateId = SMSCredentials.DLTTemplateId;

              
                string encodedText = System.Net.WebUtility.UrlEncode(text);

                string fullUrl = $"{smsApiUrl}?user={user}&password={password}&senderid={senderid}&channel={channel}&DCS={DCS}&flashsms={flashsms}&number={number}&text={encodedText}&route={route}&peid={peid}&DLTTemplateId={DLTTemplateId}";

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(fullUrl);

                if (!response.IsSuccessStatusCode)
                {
                  
                    TempData["FailureMessage"] = "Invalid PhoneNumber!";
                    return Page();
                    
                }
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"SMS send failed: {ex.Message}");
                return Page();
            }
            return Page();
        }

    }
}
