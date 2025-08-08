using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class IndianStates
    {
        [Key]
       public int  StateID { get; set; }
        public string    StateName { get; set; }
        public bool IsUnionTerritory { get; set; }
    }
}
