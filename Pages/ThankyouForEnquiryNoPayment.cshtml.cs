using Astaberry.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using static EcommerceTelysia.Pages.CheckoutModel;
using System.Net.Http.Headers;
using EcommerceTelysia.Class;
using EcommerceTelysia.Models;

using Microsoft.EntityFrameworkCore;


namespace EcommerceTelysia.Pages
{
    public class ThankyouForEnquiryNoPaymentModel : PageModel
    {
        ApplicationDbContext _context;
        public ThankyouForEnquiryNoPaymentModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public OrderDetails OrderDetails { get; private set; }
        public TblOrderId OrderId { get; set; }
        [BindProperty]
        public string OID { get; set; }
        public List<Item> Carts { get; set; } = new List<Item>();
        //*    public MailCredentials MailCredentials { get; private set; }*//*

        public double shipping { get; set; }
        public string Currenturl { get; private set; }
        public double Total { get; set; }
        public double subtotal { get; set; }

        public TblBillingDetail Detail { get; set; }
        public TblShippingDetail ShippingDetail { get; set; }

        public TblCustomerOrderDetails CustomerOrderDetails { get; set; }
        public List<BuyNow> childskuCodes { get; set; } = new List<BuyNow>();
        //* public List<OrderMail> orderDetails { get; set; } = new List<OrderMail>();*//*
        public DateTime date { get; set; }

        public string paymentMethod { get; set; }
        public double CouponDiscount { get; set; }
        public double DiscountAmount { get; set; }
        public double AfterDiscount { get; set; }
        public string Phone { get; set; }

        [BindProperty]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                bool isBuyNow = HttpContext.Session.GetString("isBuyNow") == "True";
                if (isBuyNow)
                {
                    childskuCodes = SessionHelper.GetObjectFromJson<List<BuyNow>>(HttpContext.Session, "buynowItem") ?? new List<BuyNow>();
                }
                else
                {
                    Carts = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") ?? new List<Item>();
                }

              

            
                string state = "COMPLETED";

                if (state == "COMPLETED")
                {
                    StatusMessage = "✅ Payment successful!";
                }
                else if (state == "PENDING")
                {
                    StatusMessage = "⏳ Payment is still pending.";
                }
                else
                {
                    StatusMessage = $"❌ Payment failed or cancelled. State: {state}";
                }

                var discountvalue = HttpContext.Session.GetInt32("Discount");
                string check = HttpContext.Session.GetString("AppliedCoupon");

                // Calculate totals
                CalculateTotals(isBuyNow, discountvalue, check);

                // Retrieve session data
                string email = HttpContext.Session.GetString("Email");
                string comment = HttpContext.Session.GetString("Comment");
                paymentMethod = HttpContext.Session.GetString("PaymentMethod");
                Phone = HttpContext.Session.GetString("Mobile");
                bool isShipToDifferentAddress = HttpContext.Session.GetString("IsShipToDifferentAddress") == "True";

                Detail = JsonConvert.DeserializeObject<TblBillingDetail>(HttpContext.Session.GetString("BillingDetails"));
                ShippingDetail = JsonConvert.DeserializeObject<TblShippingDetail>(HttpContext.Session.GetString("ShippingDetails"));

                // Generate Order ID using stored procedure
                string generatedOrderId = await GenerateOrderId(email, comment, paymentMethod, state,Phone);

                // Save Billing and Shipping Details
                await SaveBillingAndShippingDetails(generatedOrderId, email, isShipToDifferentAddress, Phone);

                // Save Order Details
                await SaveOrderDetails(generatedOrderId, email, isBuyNow);

                // Handle post-payment response
                return await HandlePaymentResponse(state, generatedOrderId);
            }
            catch (Exception ex)
            {
                // Optional: log the exception using a logger
                // _logger.LogError(ex, "An error occurred while processing the payment.");
                return RedirectToPage("/Error");
            }
        }




        public async Task<string> GetAccessTokenAsync()
        {
            using var client = new HttpClient();

            var values = new Dictionary<string, string>
        {
            { "client_id", PhonePeCredentials.ClientId },
            { "client_secret", PhonePeCredentials.ClientSecret },
            { "grant_type", "client_credentials" },
            { "client_version", "1" } // from production email
        };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("https://api.phonepe.com/apis/identity-manager/v1/oauth/token", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception("Access token fetch failed: " + responseBody);

            dynamic result = JsonConvert.DeserializeObject(responseBody);
            return result.access_token;
        }
        private void CalculateTotals(bool isBuyNow, int? discountvalue, string check)
        {
            subtotal = isBuyNow
                ? childskuCodes.Sum(item => (double)item.Price * item.Qty)
                : Carts.Sum(item => (double)item.Price * item.Qty);

            if (isBuyNow)
            {
                Total = subtotal;
            }
            else
            {
                if (check == "True" && discountvalue.HasValue)
                {
                    DiscountAmount = subtotal * (double)discountvalue / 100;
                    AfterDiscount = subtotal - DiscountAmount;
                    Total = AfterDiscount;
                }
                else
                {
                    AfterDiscount = subtotal;
                    Total = subtotal;
                }
            }

            // ✅ Apply new shipping logic
            if (Total < 199)
                shipping = 99;
            else if (Total >= 199 && Total < 599)
                shipping = 49;
            else
                shipping = 0;

            Total += shipping;
        }




        public async Task<string> GenerateOrderId(string email, string comment, string paymentMethod, string paymentStatus, string mobile)
        {
            string Email = email;
            string Phone = mobile??"NA";
            date = DateTime.Now;
            float couponCode = 0;
            string status = "Pending";
            float totalAmount = (float)Total;
            string paymentFrom = "Enquiry";
            string PaymentStatus =  "Pending";
            string OrderNotes = comment;

            var generatedOrderId = new SqlParameter
            {
                ParameterName = "@ProdID",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = 15,
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC SpOrderId @Date, @CouponCodeapplied, @Mobile, @Emailid, @Status, @TotalAmount, @PaymentFrom, @PaymentStatus, @OrderNotes, @ProdID OUTPUT",
                new SqlParameter("@Date", date),
                new SqlParameter("@CouponCodeapplied", couponCode),
                new SqlParameter("@Mobile", Phone),
                new SqlParameter("@Emailid", Email),
                new SqlParameter("@Status", status),
                new SqlParameter("@TotalAmount", totalAmount),
                new SqlParameter("@PaymentFrom", paymentFrom),
                new SqlParameter("@PaymentStatus", PaymentStatus),
                new SqlParameter("@OrderNotes", OrderNotes),
                generatedOrderId
            );

            return generatedOrderId.Value.ToString();
        }

        private async Task SaveBillingAndShippingDetails(string orderId, string email, bool isShipToDifferentAddress, string Phone)
        {
            Detail.ContactNumber = Phone;
            Detail.Orderid = orderId;
            Detail.Emailid = email;
            Detail.FillDefaults();
            await _context.TblBillingDetail.AddAsync(Detail);
            await _context.SaveChangesAsync();

            if (!isShipToDifferentAddress)
            {
                ShippingDetail = Detail.CloneToShipping(orderId);
            }
            else
            {
                ShippingDetail.FillDefaults(orderId, Detail);
            }

            await _context.TblShippingDetail.AddAsync(ShippingDetail);
            await _context.SaveChangesAsync();
        }

        private async Task SaveOrderDetails(string orderId, string email, bool isBuyNow)
        {
            if (isBuyNow)
            {
                foreach (var item in childskuCodes)
                {
                    var orderDetail = new TblCustomerOrderDetails
                    {
                        OrderCode = orderId,
                        ProductId = item.ProductId ?? "NA",
                        Qty = item.Qty, // For Buy Now, it's Quantity
                        Price = (float)item.Price,
                        Status = "Pending",


                        Email = email ?? "NA"
                    };
                    await _context.TblCustomerOrderDetails.AddAsync(orderDetail);
                }
            }
            else
            {
                foreach (var item in Carts)
                {
                    var orderDetail = new TblCustomerOrderDetails
                    {
                        OrderCode = orderId,
                        ProductId = item.ProductId ?? "NA",
                        Qty = item.Qty, // For Cart, it's Qty
                        Price = (float)item.Price,
                        Status = "Pending",

                        Email = email ?? "NA"
                    };
                    await _context.TblCustomerOrderDetails.AddAsync(orderDetail);
                }
            }

            await _context.SaveChangesAsync();
        }


        private async Task<IActionResult> HandlePaymentResponse(string paymentCode, string orderId)
        {
            HttpContext.Session.SetString("OrderCompleted", "true");
            HttpContext.Session.SetString("OrderId", orderId);
            OID = orderId;

            if (paymentCode == "COMPLETED")
            {
                return Page();
            }

            return RedirectToPage(paymentCode == "PAYMENT_ERROR" ? "/Error" : "/Error1");
        }
    }
}
public static class BillingDetailExtensions
{
    public static void FillDefaults(this TblBillingDetail detail)
    {
        detail.Apartment ??= "NA";
        detail.LastName ??= "NA";
        detail.Gst ??= "3";
        detail.Name ??= "NA";
        detail.Country ??= "India";
        detail.ContactNumber ??= "0000000000";
        detail.City ??= "NA";
        detail.State ??= "NA";
        detail.Address ??= "NA";
        detail.PinCode ??= "000000";
    }

    public static TblShippingDetail CloneToShipping(this TblBillingDetail billing, string orderId)
    {
        return new TblShippingDetail
        {
            Orderid = orderId,
            Apartment = billing.Apartment,
            LastName = billing.LastName,
            Name = billing.Name,
            Country = billing.Country,
            City = billing.City,
            State = billing.State,
            Address = billing.Address,
            Emailid = billing.Emailid,
            PinCode = billing.PinCode,
            ContactNumber = billing.ContactNumber
        };
    }

    public static void FillDefaults(this TblShippingDetail shipping, string orderId, TblBillingDetail billing)
    {
        shipping.Orderid = orderId;
        shipping.Apartment ??= billing.Apartment;
        shipping.LastName ??= billing.LastName;
        shipping.Name ??= billing.Name;
        shipping.Country ??= billing.Country;
        shipping.City ??= billing.City;
        shipping.State ??= billing.State;
        shipping.Address ??= billing.Address;
        shipping.Emailid ??= billing.Emailid;
        shipping.PinCode ??= billing.PinCode;
        shipping.ContactNumber ??= billing.ContactNumber;
    }
}


