using EcommerceTelysia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using EcommerceTelysia.Models;
using EcommerceTelysia.Pages;
using EcommerceTelysia;
using EcommerceTelysia.Models;


namespace EcommerceTelysia.Models
{





    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public virtual ApplicationDbContext Context { get; set; }
        public DbSet<User> TblUsers { get; set; }
        public DbSet<AdminLogin> TblAdminLogin { get; set; }
        public DbSet<RoleNames> TblRoleNames { get; set; }
        public DbSet<Registration> TblRegisteration { get; set; }
      
        public DbSet<AdminPackageConfiguration> TblAdminPackageConfiguration { get; set; }
    

        public DbSet<CouponCodes> TblCouponCodes { get; set; }
        public DbSet<Cart> TblCart { get; set; }
       
        public DbSet<AssignedRoleToUser> TblAssignedRoleToUser { get; set; }
        public DbSet<UserLoggedInReports> TblUserLoggedInReports { get; set; }
        public DbSet<Ticket> TblTicket { get; set; }
        public DbSet<InternalNote> TblInternalNotes { get; set; }
       
        public DbSet<Reviews> TblReviews { get; set; }
        public DbSet<TblCustomerOrderDetails> TblCustomerOrderDetails { get; set; }
        public DbSet<Enquiry> TblEnquiry { get; set; }
        public DbSet<FaultRepotingForUsers> TblFaultRepotingForUsers { get; set; }
        public DbSet<Leads> TblLeads { get; set; }
       
        public DbSet<TblBillingDetail> TblBillingDetail { get; set; }
        public DbSet<TblShippingDetail> TblShippingDetail { get; set; }
        public DbSet<TblOrderId> TblOrderId { get; set; }
      
        public DbSet<WareHouse> TblWareHouse { get; set; }
        public DbSet<Inventory> TblInventory { get; set; }
        public DbSet<StockInOut> TblStockInOut { get; set; }
        public DbSet<DamagedProducts> TblDamagedProducts { get; set; }
        public DbSet<IndianStates> TblIndianStates { get; set; }
      
        public DbSet<StatewiseElectricityPrice> TblStatewiseElectricityPrice { get; set; }
        public DbSet<RegisterForEcommerce> TblRegisterForEcommerce { get; set; }

        public DbSet<Products> TblProducts { get; set; } = default!;
        public DbSet<OTPVerification> TblOTPVerification { get; set; } = default!;




    }



        


}




