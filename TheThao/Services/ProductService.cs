using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using TheThao.Services;

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
            .ThenInclude(ctkm => ctkm.MaKmNavigation) // Include latest promotion
            .ToList(); // Load all products into memory

        foreach (var product in products)
        {
            // Lấy phần trăm giảm giá mới nhất
            var latestPercentage = product.ChiTietPhanTrams
                .OrderByDescending(ctpt => ctpt.Id)
                .FirstOrDefault()?.MaPhanTramNavigation?.PhanTram ?? 0;

            // Lấy khuyến mãi hiện tại (nếu có và chưa hết hạn)
            var latestPromotion = product.ChiTietKhuyenMais
                .Where(ctkm => ctkm.NgayKetThuc >= DateTime.Now)
                .OrderByDescending(ctkm => ctkm.Id)
                .FirstOrDefault()?.MaKmNavigation?.PhanTramKhuyenMai ?? 0;

            // Lấy giá nhập hàng mới nhất
            var latestPurchasePrice = product.ChiTietNhapHangs
                .OrderByDescending(ctnh => ctnh.Id)
                .FirstOrDefault()?.Gia;

            // Nếu có giá nhập hàng và có khuyến mãi mới
            if (latestPurchasePrice.HasValue && latestPromotion >= 0)
            {
                var newPrice = latestPurchasePrice * (1 + (decimal)latestPercentage / 100) * (1 - (decimal)latestPromotion / 100);

                // Áp dụng giá mới và cập nhật GiaTruocKhiGiamGia
                product.Gia = newPrice;
                product.GiaTruocKhiGiamGia = latestPurchasePrice * (1 + (decimal)latestPercentage / 100);
            }
        }

        // Lưu các thay đổi vào cơ sở dữ liệu
        _context.SaveChanges();
    }
}
