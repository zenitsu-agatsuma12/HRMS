using HRMSAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HRMSAPI.Data
{
    public class HRMSDBContext : IdentityDbContext<ApplicationUser>
    {
        public IConfiguration _appConfig { get; }
        public ILogger _logger { get; }
        public IWebHostEnvironment _env { get; }  

        public HRMSDBContext(IConfiguration appConfig, ILogger<HRMSDBContext> logger, IWebHostEnvironment env)
        {
            _appConfig = appConfig;
            _logger = logger;
            _env = env;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var server = _appConfig.GetConnectionString("Server");
            var db = _appConfig.GetConnectionString("DB");
           
            string connectionString;
            if (_env.IsDevelopment())
            {
                connectionString = $"Server={server};Database={db};MultipleActiveResultSets=true;Integrated Security=false;TrustServerCertificate=true;";
            } else
            {
                var userName = _appConfig.GetConnectionString("UserName");
                var password = _appConfig.GetConnectionString("Password");
                connectionString = $"Server={server};Database={db};User Id= {userName};Password={password};MultipleActiveResultSets=true;Integrated Security=false;TrustServerCertificate=true;";
            }

            if (string.IsNullOrEmpty(connectionString)) {
                throw new ArgumentNullException("Connection string is not configured.");
            }

            optionsBuilder.UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            })
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

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

    }
}

