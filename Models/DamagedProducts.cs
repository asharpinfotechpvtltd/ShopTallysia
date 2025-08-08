using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class DamagedProducts
    {

        [Key]
        public int DamageReportId { get; set; }
        [ForeignKey("Products")]
        public int ProductId { get; set; }
   
        public string SKU { get; set; }

        
        public string ProductName { get; set; }

        
        public int DamagedQuantity { get; set; }

        
        public string DamageReason { get; set; } 

        [ForeignKey("TblWarehouseMaster")]
        public int WarehouseId { get; set; }

        public WareHouse TblWarehouseMaster { get; set; }
        public Products Products { get; set; }
        public DateTime ReportedOn { get; set; } = DateTime.Now;
    }
}