namespace EcommerceTelysia.Models
{
    public class StatewiseElectricityPrice
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public double ResidentailPrice { get; set; }
        public double CommercialPrice { get; set; }
        public double IndustrialPrice { get; set; }
    }

}
