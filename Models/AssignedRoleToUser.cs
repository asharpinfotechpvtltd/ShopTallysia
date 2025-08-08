using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceTelysia.Models
{
    public class AssignedRoleToUser
    {
        [Key]
        public int Id { get;set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
        public RoleNames Role { get;set; }
    }
}
