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
            .ThenInclude(navigationPropertyPath: ctpt => ctpt.MaPhanTramNavigation) // Include latest percentage discount
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

                // Lấy phần trăm giảm giá mới nhất
                
                // Lấy phần trăm khuyến mãi mới nhất
                var latestPromotionPercentage = product.ChiTietKhuyenMais
                    .OrderByDescending(ctkm => ctkm.Id)
                    .FirstOrDefault()?.MaKmNavigation?.PhanTramKhuyenMai ?? 0; // Mặc định là 1 nếu không có khuyến mãi

                // Lấy giá nhập hàng mới nhất
                

                // Cập nhật giá mới cho sản phẩm
                if (latestPurchasePrice.HasValue)
                {
                    var newPrice = latestPurchasePrice.Value * (1 + (decimal)latestPercentage / 100) * (1 - (decimal)latestPromotionPercentage / 100);

                    // Cập nhật giá mới cho sản phẩm
                    product.Gia = newPrice;
                    product.GiaTruocKhiGiamGia = latestPurchasePrice * (1 + (decimal)latestPercentage / 100);
                }

               
                

            }
        }



        






        _context.SaveChanges();

    }
    public void Cost()
    {
        var product = _context.SanPhams.ToList();
        foreach (var item in product)
        {
            item.GiaSi = item.Gia * 0.9M;

        }
        _context.SaveChanges();

    }
}
