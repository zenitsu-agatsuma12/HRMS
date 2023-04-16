using HRMS.Models;
using HRMS.ViewModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HRMS.Data
{
    public static class SeedData
    {
        public static void SeedDefaultData(this ModelBuilder modelBuilder, IDataProtectionProvider dataProtectionProvider)
        { 

            modelBuilder.Entity<Department>().HasData(
                new Department(1, "Human Resource"),
                new Department(2, "Information Technology")
                ) ;
            modelBuilder.Entity<Position>().HasData(
                new Position(1, "Administrative Assistant"),
                new Position(2, "Team Leader"),
                new Position(3, "Associate")
                );

            modelBuilder.Entity<Employee>().HasData(
                new Employee(1, "Alvin", "Eleuterio", "Root", "Alvin Eleuterio Root", "Male", DateTime.Now.AddDays(1),"alvin@gmail.com", "09952610728", "alvin", "alvin", 2, 2, "P. Laygo St.", "Sabang", "Lipa City", "Batangas", 4217, true, "Regular", DateTime.Now.AddDays(1)),
                new Employee(2, "Earl Joseph", "Litong", "Ferran", "Earl Joseph Litong Ferran", "Male", DateTime.Now.AddDays(1), "earl@gmail.com", "09657610728", "earl", "earl", 2, 1, "P. Laygo St.", "Sabang", "Lipa City", "Mindoro", 4217, true, "Regular", DateTime.Now.AddDays(2)),
                new Employee(3, "Coco", "Mama", "Martin", "Coco Mama Martin", "Male", DateTime.Now.AddDays(1), "cocomama@gmail.com", "09127610728", "coco", "coco", 1, 2, "P. Laygo St.", "Sabang", "Lipa City", "Mindoro", 4217, true, "Regular", DateTime.Now.AddDays(3))
                );

            var roleid = Guid.NewGuid().ToString();
            var mroleid = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole {Id = roleid, Name = "Administrator",NormalizedName = "ADMINISTRATOR"},

                new IdentityRole{Id = Guid.NewGuid().ToString(),Name = "Employee",NormalizedName = "EMPLOYEE"},

                new IdentityRole{Id = mroleid, Name = "Manager",NormalizedName = "MANAGER"}
                );

            var userId= Guid.NewGuid().ToString();
            // var managerId= Guid.NewGuid().ToString();
            var protector = dataProtectionProvider.CreateProtector("Email");
            var sssProtector = dataProtectionProvider.CreateProtector("SSSNumber");

            modelBuilder.Entity<ApplicationUser>().HasData(
                 new ApplicationUser
                 {
                     Id = userId,
                     FirstName = "Admin",
                     MiddleName = "Admin",
                     LastName = "Admin",
                     FullName = "Administrator",
                     DepartmentId=1,
                     Gender = "Male",
                     DateOfBirth = DateTime.Now,
                     Email = protector.Protect("admin@admin.com"),
                     SSSNumber = sssProtector.Protect("1111111111111"),
                     PagIbigId = "111111111111",
                     PhilHealthId = "111111111111",
                     NormalizedEmail = "ADMIN@ADMIN.COM",
                     EmailConfirmed = true,
                     UserName = "admin@admin.com",
                     NormalizedUserName = "ADMIN@ADMIN.COM",
                     Phone = "09111111111",
                     Street = "Admin",
                     Barangay = "Admin",
                     City = "Admin",
                     State = "Admin",
                     PostalCode = 1,
                     ActiveStatus=true,

                     PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "admin"),
                     SecurityStamp = String.Empty,
                 }
                 );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { UserId = userId, RoleId = roleid } // Admin user has Admin role
                  //  new IdentityUserRole<string> { UserId = managerId, RoleId = mroleid }
                );
           
        }
    }
}
