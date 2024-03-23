using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheThao.Services
{
    public class PriceUpdateService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public PriceUpdateService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Do your price update logic here
                using (var scope = _services.CreateScope())
                {
                    var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                    productService.UpdateProductPrices();
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Update prices every 24 hours
            }
        }
    }
}
