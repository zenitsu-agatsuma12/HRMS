using HRMS.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HRMS.Data
{
    public class HRMSDBContext : IdentityDbContext<ApplicationUser>
    {
        public IConfiguration _appConfig { get; }
        public ILogger _logger { get; }
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public HRMSDBContext(IConfiguration appConfig, ILogger<HRMSDBContext> logger, IDataProtectionProvider dataProtectionProvider)
        {
            _appConfig = appConfig;
            _logger = logger;
            _dataProtectionProvider = dataProtectionProvider;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string con = "Server = (localdb)\\MSSQLLocalDB; Database = HRMSDB; Integrated Security = true;";
            optionsBuilder.UseSqlServer(con).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<EmployeePerformance> EmployeePerformances { get; set; }
        public DbSet<SSS> SSSs { get; set; }
        public DbSet<PhilHealth> PhilHealths { get; set; }
        public DbSet<PagIbig> pagIbigs { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressesTypes { get; set; }
        public DbSet<SSSPayment> SSSPayments { get; set; }
        public DbSet<PhilHealthPayment> PhilHealthPayments { get; set; }
        public DbSet<PagIbigPayment> PagIbigPayments { get; set; }


        // Many to Many
        public DbSet<DepartmentPositioncs> DepartmentPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SeedDefaultData(_dataProtectionProvider);
            base.OnModelCreating(modelBuilder);
        }
      
    }
}

