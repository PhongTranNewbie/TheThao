using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheThao.Data;
using TheThao.Services;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<TheThaoContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add session
        builder.Services.AddSession(); // Thêm dịch vụ session vào trong ConfigureServices
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
            pattern: "{controller=Login}/{action=Client}/{id?}");

        // Seed data vào cơ sở dữ liệu
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<TheThaoContext>();
            SeedData.Initialize(context);
        }

        app.Run();
    }
}