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
              .ThenInclude(ctpt => ctpt.MaPhanTramNavigation) // Include latest percentage discount
              .Include(p => p.ChiTietNhapHangs)
              .ThenInclude(ctnh => ctnh.MaNhapHangNavigation)
              .Include(p => p.ChiTietKhuyenMais)
              .ThenInclude(ctkm => ctkm.MaKmNavigation)// Include latest purchase price
              .ToList(); // Load all products into memory

            foreach (var product in products)
            {
                // Lấy phần trăm giảm giá mới nhất
                var latestPercentage = product.ChiTietPhanTrams
                  .OrderByDescending(ctpt => ctpt.Id)
                  .FirstOrDefault()?.MaPhanTramNavigation?.PhanTram ?? 0;
                // Lấy phần trăm khuyến mãi mới nhất
                var latestPromotionPercentage = product.ChiTietKhuyenMais
                    .OrderByDescending(ctkm => ctkm.Id)
                    .FirstOrDefault()?.MaKmNavigation?.PhanTramKhuyenMai ?? 0; // Mặc định là 1 nếu không có khuyến mãi

                // Lấy giá nhập hàng mới nhất
                var latestPurchasePrice = product.ChiTietNhapHangs
                  .OrderByDescending(ctnh => ctnh.Id)
                  .FirstOrDefault()?.Gia;

                // Cập nhật giá mới cho sản phẩm
                if (latestPurchasePrice.HasValue)
                {
                    var newPrice = latestPurchasePrice * (1 + (decimal)latestPercentage / 100) * (1 - (decimal)latestPromotionPercentage / 100);

                    // Cập nhật giá mới cho sản phẩm
                    product.Gia = newPrice;
                }

            }

            _context.SaveChanges();
        }

       
        



    }
}
