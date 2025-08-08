namespace EcommerceTelysia.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateOnly Dob { get; set; }
        public string District { get; set; }
        public int PinCode { get; set; }
        public string altMobile { get; set; }
        public string Gender { get; set; }
        public string Category { get; set; }
        public string Password { get; set; }
        public string RegistrationNumber { get; set; }
    }

}
