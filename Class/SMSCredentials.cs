namespace EcommerceTelysia.Class
{
    public class SMSCredentials
    {
        public static readonly string smsApiUrl = "http://smsfortius.in/api/mt/SendSMS";
        public static readonly string user = "tallysiaenergies";
        public static readonly string password = "tallysiaenergies";
        public static readonly string senderid = "TALSIA";
        public static readonly string channel = "Trans";
        public static readonly string DCS = "0";
        public static readonly string flashsms = "0";
        public static readonly string expiry = "10 mins";
        public static readonly string route = "02";
        public static readonly string peid = "1701174988674580232";
        public static readonly string DLTTemplateId = "1707175083269388326";

        public static string AccessToken { get; set; }
    }
}
