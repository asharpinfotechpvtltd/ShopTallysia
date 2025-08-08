/*namespace CrystalByRiya
{
    public static class PhonePeCredientials
    {
        public readonly static string RedirectUrl = "https://www.crystalsbyriya.com/thankyou";
        //public readonly static string RedirectUrl = "https://localhost:44333/thankyou";
        public readonly static string CallbackUrl = "https://www.crystalsbyriya.com/thankyou";
        //public readonly static string CallbackUrl = "https://localhost:44333/api/response";
        //public readonly static string SaltKey = "14fa5465-f8a7-443f-8477-f986b8fcfde9";
        public readonly static string SaltKey = "66222d9f-cfd6-41fd-8b83-d495f887a63e";
        public readonly static string PostUrl = "https://api.phonepe.com/apis/hermes/pg/v1/pay";
        //  public readonly static string PostUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/pg/v1/pay";
        public readonly static string Merchantid = "CRYSTALSONLINE";
        public readonly static int saltIndex = 1;
        public static string OrderId { get; set; }
        public static string xverify { get; set; }


        public static string checkstatusPhonePeGatewayURL = "https://api.phonepe.com/apis/hermes";
        //8468977350
    }
}*/
namespace EcommerceTelysia
{
    /*   public static class PhonePeCredientials
       {
           // public readonly static string RedirectUrl = "https://www.crystalsbyriya.com/thankyou";
           public readonly static string RedirectUrl = "https://localhost:44379/Thankyou";
           //public readonly static string CallbackUrl = "https://www.crystalsbyriya.com/thankyou";
           public readonly static string CallbackUrl = "https://localhost:44379/api/response";
           public readonly static string SaltKey = "32011a23-6bab-4d91-ae48-f68f485ef7c7";
           public readonly static string PostUrl = "https://api.phonepe.com/apis/hermes/pg/v1/pay";
           // public readonly static string Merchantid = "CRYSTALSONLINE";
           public readonly static int saltIndex = 1;
           public static string OrderId { get; set; }
           public static string xverify { get; set; }
           public static string Merchantid { get; set; } = "SU2505081810282459155731";
       }*/
  
        public static class PhonePeCredentials
        {
            public static readonly string RedirectUrl = "https://localhost:44379/Thankyou";
            public static readonly string CallbackUrl = "https://localhost:44379/api/response";
            public static readonly string AuthUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";
            public static readonly string PaymentUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/pg/v1/pay";
            public static readonly string ClientId = "SU2505081810282459155731";
            public static readonly string ClientSecret = "32011a23-6bab-4d91-ae48-f68f485ef7c7";
            public static string AccessToken { get; set; }
        }
    

}
