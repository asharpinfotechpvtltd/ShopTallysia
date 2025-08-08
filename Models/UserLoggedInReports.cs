using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class UserLoggedInReports
    {
        [Key]
        public int Id { get;set; }
        public string UserEmail { get; set; }
       
        public DateTime LoggedInDate { get; set; }
    }
}
