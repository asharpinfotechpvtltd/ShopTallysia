using Astaberry.Helpers;
using EcommerceTelysia.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

using EcommerceTelysia.Class;
using EcommerceTelysia.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceTelysia.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly PhonePePaymentService _paymentService;


        private readonly ApplicationDbContext _context;

        public CheckoutModel(ApplicationDbContext context, PhonePePaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }
        public class OrderDetails
        {
            public string OrderId { get; set; }
            public string username { get; set; }
            public string Phone { get; set; }
            public string PaymentMethod { get; set; }
            public double Subtotal { get; set; }
            public string Shippingaddress { get; set; }
            public string Billingaddress { get; set; }
            public double Total { get; set; }
            public List<Item> Items { get; set; }

            public string comment { get; set; }

            public DateTime OrderDate { get; set; }
        }
        [BindProperty]
        public Register register { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblBillingDetail Detail { get; set; }
        [BindProperty(SupportsGet = true)]
        public TblShippingDetail ShippingDetail { get; set; }
        public TblCustomerOrderDetails CustomerOrderDetails { get; set; }

        public string Mobile { get; private set; }
        public string Password { get; private set; }

        public string Currenturl { get; private set; }
        public List<Item> Carts { get; set; } = new List<Item>();
        public List<BuyNow> childskuCodes { get; set; } = new List<BuyNow>();

        public double GstAmount { get; set; }
        public double subtotal { get; set; }
        public double shipping { get; set; }
        public double Total { get; set; }
        public double CouponDiscount { get; set; }
        public double DiscountAmount { get; set; }
        public double AfterDiscount { get; set; }

        public bool NeedsToAddDetails { get; set; }
        public SelectList StateNames { get; set; } = default!;
        public async Task OnGet(bool isBuyNow = false)
        {
            var states = await _context.TblIndianStates.ToListAsync();
            StateNames = new SelectList(states, "StateName", "StateName");
            HttpContext.Session.SetString("isBuyNow", isBuyNow.ToString());
           
            Currenturl = HttpContext.Request.GetDisplayUrl();
            var discountvalue = HttpContext.Session.GetInt32("Discount");
            var check = HttpContext.Session.GetString("AppliedCoupon");

            HttpContext.Session.Remove("CouponCode");
            Mobile = HttpContext.Session.GetString("Mobile");

            if (isBuyNow)
            {
               
                childskuCodes = SessionHelper.GetObjectFromJson<List<BuyNow>>(HttpContext.Session, "buynowItem");


                if (childskuCodes != null)
                {
                  
                    subtotal = childskuCodes.Sum(item => item.Price * item.Qty);


                    shipping = subtotal < 3000 ? 80 : 0; 
                    Total = subtotal + shipping;

               
                   Mobile = HttpContext.Session.GetString("Mobile");
                    if (!string.IsNullOrEmpty(Mobile))
                    {
                        Detail = _context.TblBillingDetail.Where(b => b.ContactNumber == Mobile).FirstOrDefault();
                        if (Detail == null)
                        {
                            Detail = new TblBillingDetail { ContactNumber = Mobile };
                        }
                        ShippingDetail = _context.TblShippingDetail.Where(s => s.ContactNumber == Mobile).FirstOrDefault();
                        if (ShippingDetail == null)
                        {
                            ShippingDetail = new TblShippingDetail { ContactNumber=Mobile };
                        }
                    }
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "buynowItem", childskuCodes);
            }
            else
            {
           
                Carts = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

                if (Carts != null && Carts.Any())
                {
                    subtotal = Carts.Sum(item => item.Price * item.Qty);
                    if (check == "True")
                    {
                        DiscountAmount = subtotal * (double)discountvalue / 100;
                        AfterDiscount = subtotal;
                        AfterDiscount -= DiscountAmount;
                        shipping = AfterDiscount < 3000 ? 80 : 0; 
                        Total = AfterDiscount + shipping;
                    }
                    else
                    {
                        shipping = subtotal < 3000 ? 80 : 0;
                        Total = subtotal + shipping;

                    }
                }

                // Retrieve user session information if the user is logged in
           Mobile = HttpContext.Session.GetString("Mobile");
                if (!string.IsNullOrEmpty(Mobile))
                {
                    Detail = _context.TblBillingDetail.Where(b => b.ContactNumber == Mobile).FirstOrDefault();
                    if (Detail == null)
                    {
                        Detail = new TblBillingDetail { ContactNumber = Mobile };
                    }
                    ShippingDetail = _context.TblShippingDetail.Where(s => s.ContactNumber == Mobile).FirstOrDefault();
                    if (ShippingDetail == null)
                    {
                        ShippingDetail = new TblShippingDetail { ContactNumber = Mobile };
                    }
                }

            }
        }
        public async Task<IActionResult> OnPostPlaceOrder(string Phone, string comment, string paymentMethod, bool shipment, string email)
        {

            double subtotal = 0;
            double shipping = 0;
            double Total = 0;
            bool isBuyNow = HttpContext.Session.GetString("isBuyNow") == "True";
            var Discount = HttpContext.Session.GetInt32("Discount");
            var checkvalue = HttpContext.Session.GetString("AppliedCoupon");

            if (isBuyNow)
            {
                // Retrieve Buy Now item(s)
                childskuCodes = SessionHelper.GetObjectFromJson<List<BuyNow>>(HttpContext.Session, "buynowItem");
            }
            else
            {
                // Retrieve cart items
                Carts = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            }

            // Calculate subtotal and shipping based on the selected items
            if (Carts != null && Carts.Any())
            {
                foreach (var item in Carts)
                {
                    subtotal += item.Price * item.Qty;
                }
                if (checkvalue == "True")
                {
                    DiscountAmount = subtotal * (double)Discount / 100;
                    AfterDiscount = subtotal - DiscountAmount;
                    shipping = AfterDiscount < 3000 ? 80 : 0;
                    Total = AfterDiscount + shipping;



                }
                else
                {
                    shipping = subtotal < 3000 ? 80 : 0;
                    Total = subtotal + shipping;
                }

            }
            else if (childskuCodes != null)
            {
                subtotal += childskuCodes.Sum(item => item.Price * item.Qty);
                shipping = subtotal < 3000 ? 80 : 0;
                Total = subtotal + shipping;

            }



            // Apply shipping if necessary

            // Calculate the final total

            HttpContext.Session.SetString("Phone", Phone);
            HttpContext.Session.SetString("isBuyNow", isBuyNow.ToString());
            SessionHelper.SetObjectAsJson(HttpContext.Session, "buynowItem", childskuCodes);


            HttpContext.Session.SetString("Comment", comment ?? "NA");
            HttpContext.Session.SetString("PaymentMethod", paymentMethod);
            HttpContext.Session.SetString("Email", email);


            HttpContext.Session.SetString("IsShipToDifferentAddress", shipment.ToString());

            // Store other form data for Billing and Shipping details in the session
            HttpContext.Session.SetString("BillingDetails", JsonConvert.SerializeObject(Detail));
            HttpContext.Session.SetString("ShippingDetails", JsonConvert.SerializeObject(ShippingDetail));


            // Check if the payment method is 'cod' (Cash on Delivery)
            decimal amount;
            if (paymentMethod == "cod")
            {
                amount = (decimal)Total;
               return RedirectToPage("ThankyouForEnquiryNoPayment");
            }
            else
            {
                // For online payment, use the total amount calculated from the cart or Buy Now item
                amount = (decimal)Total;
            }
            double integerAmount = Convert.ToDouble(Math.Round(amount, 2) * 100);
            string token = await GetAccessTokenAsync(); // Reuse from step 1

            string orderId = "TXN" + new Random().Next(100000, 999999); // your unique merchantOrderId

            var requestData = new
            {
                merchantId = "M220XDXH5HP4J",
                merchantOrderId = orderId,
                merchantUserId = "user_" + orderId,
                amount = integerAmount,
                expireAfter = 1200,
                metaInfo = new
                {
                    udf1 = "additional-information-1",
                    udf2 = "additional-information-2",
                    udf3 = "additional-information-3",
                    udf4 = "additional-information-4",
                    udf5 = "additional-information-5"
                },
                paymentInstrument = new
                {
                    type = "PAY_PAGE"
                },
                paymentFlow = new
                {
                    type = "PG_CHECKOUT",
                    message = "Payment message used for collect requests",
                    merchantUrls = new
                    {
                        redirectUrl = "https://anandavrinda.com/PhonePeSuccess?orderId=" + orderId
                    }
                }
            };



            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("O-Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.phonepe.com/apis/pg/checkout/v2/pay", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response:\n" + responseContent);

            if (!response.IsSuccessStatusCode)
                return Content("Create Order Failed:\n" + responseContent);

            var jsonObj = JObject.Parse(responseContent);
            string paymentToken = jsonObj["token"]?.ToString();
            string redirectUrl = jsonObj["redirectUrl"]?.ToString();

            if (string.IsNullOrEmpty(redirectUrl))
                return Content("Redirect URL not found in response.");

            return Redirect(redirectUrl);


        }
        public async Task<string> GetAccessTokenAsync()
        {
            using var client = new HttpClient();

            var values = new Dictionary<string, string>
    {
        { "client_id", PhonePeCredentials.ClientId },
        { "client_secret", PhonePeCredentials.ClientSecret },
        { "grant_type", "client_credentials" },
        { "client_version", "1" }
    };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.phonepe.com/apis/identity-manager/v1/oauth/token", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Token Error: {responseBody}");
                throw new Exception("Failed to get access token");
            }

            dynamic result = JsonConvert.DeserializeObject(responseBody);
            PhonePeCredentials.AccessToken = result.access_token;

            return PhonePeCredentials.AccessToken;
        }






        private static string Sha256Hash(string value)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task<IActionResult> OnGetVerifyPaymentAsync(string orderId)
        {
            var verificationResponse = await _paymentService.VerifyPaymentAsync(orderId);

            // Handle verification response and update order status
            return Page();
        }
    }
}
