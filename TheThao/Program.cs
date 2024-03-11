
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using TheThao.Services;
using TheThao.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        
        builder.Services.AddDbContext<TheThaoContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Add session
        builder.Services.AddSession(); // Thêm dịch vụ session vào trong ConfigureServices
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        


        var app = builder.Build();


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthorization();

        // Add session middleware
        app.UseSession(); // Kích hoạt session trong pipeline middleware



        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.UseExceptionHandler("/Home/Error");
        // Seed data vào cơ sở dữ liệu

        app.Run();
    }
}
