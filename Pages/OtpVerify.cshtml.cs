using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MimeKit;
using MailKit.Net.Smtp;
using EcommerceTelysia.Models;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using EcommerceTelysia.Class;

namespace EcommerceTelysia.Pages
{
    public class OtpVerifyModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OtpVerifyModel(ApplicationDbContext context)
        {
            _context = context;
        }
     

        public async Task OnGet()
        {
            
        }
        public async Task<IActionResult> OnPostVerify(int number)
        {
            var cutoff = DateTime.Now.AddMinutes(-10);
            var expiredOtps = _context.TblOTPVerification.Where(o => o.Date < cutoff).ToList();
            _context.TblOTPVerification.RemoveRange(expiredOtps);
            await _context.SaveChangesAsync();
            string mobile = HttpContext.Session.GetString("Mobile");
            var check=await _context.TblOTPVerification.Where(e=>e.Number==number && e.MobileNo==mobile).FirstOrDefaultAsync();
            var check1 = await _context.TblOTPVerification.Where(e => e.Number == number ).FirstOrDefaultAsync();
            var check2 = await _context.TblOTPVerification.Where(e=> e.MobileNo == mobile).FirstOrDefaultAsync();

            if (check != null)
            {
                _context.TblOTPVerification.Remove(check);
                await _context.SaveChangesAsync();
              return  RedirectToPage("shop");

            }
            if (check1 != null)
            {
                _context.TblOTPVerification.Remove(check1);
                await _context.SaveChangesAsync();
                TempData["OtpFail"] = "Invalid Otp Try Again";
                return Page();

            }
            if (check2 != null)
            {
                _context.TblOTPVerification.Remove(check2);
                await _context.SaveChangesAsync();
                TempData["OtpFail"] = "Invalid Otp Try Again";
                return Page();

            }
            TempData["OtpFail"] = "Invalid Otp Try Again";
            return Page();  


        }
        public async Task<IActionResult> OnPostResendOtp()
        {
            string mobile = HttpContext.Session.GetString("Mobile");
            string otp = await GenerateOtp();
            await SendOtpSmsAsync(mobile, otp);
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
                Date = DateTime.Now,

            };
            await _context.TblOTPVerification.AddAsync(add);
            await _context.SaveChangesAsync();

            return otpStr;
        }

        private async Task<IActionResult> SendOtpSmsAsync(string mobile, string otp)
        {
            try
            {
                string smsApiUrl = SMSCredentials.smsApiUrl;
                string user = SMSCredentials.user;
                string password = SMSCredentials.password;
                string senderid = SMSCredentials.senderid;
                string channel = SMSCredentials.channel;
                string DCS = SMSCredentials.DCS;
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

