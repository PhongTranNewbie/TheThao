using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;

namespace TheThao.Services
{
    public class ProductService : IProductService
    {
        private readonly TheThaoContext _context;

        public ProductService(TheThaoContext context)
        {
            _context = context;
        }


        public void UpdateProductPrices()
        {
            var products = _context.SanPhams
                .Include(p => p.ChiTietPhanTrams)
                .ThenInclude(ctpt => ctpt.MaPhanTramNavigation); // Eager loading

            foreach (var product in products)
            {
                // Get the ChiTietPhanTram with the newest NgayBatDau
                var latestChiTietPhanTram = product.ChiTietPhanTrams
                    .OrderByDescending(ctpt => ctpt.NgayBatDau)
                    .FirstOrDefault();

                if (latestChiTietPhanTram != null)
                {
                    var latestPercentage = latestChiTietPhanTram.MaPhanTramNavigation?.PhanTram;

                    if (latestPercentage.HasValue)
                    {
                        // Use conditional logic to handle missing purchase history or null Gia values
                        var latestPurchasePrice = product.ChiTietNhapHangs
                            .OrderByDescending(ctnh => ctnh.MaNhapHangNavigation?.NgayNhapHang)
                            .Select(ctnh => ctnh.Gia) // Get Gia values, even if null
                            .FirstOrDefault();

                        if (latestPurchasePrice.HasValue)
                        {
                            var newPrice = latestPurchasePrice.Value * (1 + (decimal)latestPercentage / 100);
                            product.Gia = newPrice;
                        }
                        else
                        {
                            // Handle the case where there's no purchase history for the product
                            // (e.g., set a default price, log a warning, etc.)
                            Console.WriteLine($"Warning: No purchase history found for product {product.Id}");
                        }
                    }
                }
            }

            _context.SaveChanges();
        }


    }
}
