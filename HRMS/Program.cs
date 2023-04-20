using HRMS.Data;
using HRMS.Models;
using HRMS.Repository;
using HRMS.Repository.SqlRepository;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HRMSDBContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HRMSDBContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 1;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
});
builder.Services.AddScoped<HRMSDBContext, HRMSDBContext>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeDBRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentDBRepository>();
builder.Services.AddScoped<IDepartmentPositionRepository, DepartmentPositionDBRepository>();
builder.Services.AddScoped<IEmployeePerformanceDBRepository, EmployeePerformanceDBRepository>();
builder.Services.AddScoped<IPositionRepository, PositionDBRepository>();
builder.Services.AddScoped<ISSSPaymentRepository, SSSPaymentDBRepository>();
builder.Services.AddScoped<IPhilHealthPaymentDBRepository, PhilHealthPaymentDBRepository>();
builder.Services.AddScoped<IPagIbigPaymentRepository, PagIbigPaymentDBRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.Automigrate();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
