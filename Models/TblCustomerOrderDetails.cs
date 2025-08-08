using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class TblCustomerOrderDetails
    {
        public int Id { get; set; }
        public string OrderCode {  get; set; }
        public string ProductId { get; set; }
        public int Qty { get; set; }

        public double Price {  get; set; }

        public string Status { get; set; }
        public double Gst {  get; set; }
     
        public string Email { get; set; }
    }
}
