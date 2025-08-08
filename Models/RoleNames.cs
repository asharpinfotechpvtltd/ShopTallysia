using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceTelysia.Models
{
   
        public class RoleNames
        {
        [Key]
            public int RoleId { get; set; }
     public string RoleName { get; set; }

            public string UrlToAccess { get; set; }
      
    }
    

}
