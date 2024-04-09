using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TheThao.Models;
using TheThao.Services;

public class PromotionService : IPromotionService
{
    private readonly TheThaoContext _context;

    public PromotionService(TheThaoContext context)
    {
        _context = context;
    }

    public void ApplyPromotion()
    {
        // Lấy tất cả sản phẩm từ cơ sở dữ liệu
        var products = _context.SanPhams
            .Include(p => p.ChiTietKhuyenMais)
            .ThenInclude(ctpt => ctpt.MaKmNavigation)
            .ToList(); // Include latest percentage discount

        foreach (var product in products)
        {
            // Lấy mã sản phẩm của sản phẩm hiện tại
            var productId = product.Id;

            // Lấy chi tiết khuyến mãi gần nhất cho sản phẩm (nếu có)
            var latestPromotion = _context.ChiTietKhuyenMais
                .Where(c => c.MaSanPham == productId && c.NgayBatDau <= DateTime.Now)
                .OrderByDescending(c => c.NgayBatDau)
                .FirstOrDefault();

            // Nếu có khuyến mãi cho sản phẩm và chưa hết hạn
            if (latestPromotion != null && latestPromotion.NgayKetThuc >= DateTime.Now)
            {
                // Tính toán giảm giá dựa trên phần trăm khuyến mãi của chi tiết khuyến mãi
                int percentDiscount = latestPromotion.MaKmNavigation.PhanTramKhuyenMai;

                // Áp dụng giảm giá cho sản phẩm
                product.Gia -= (product.Gia * percentDiscount) / 100;
                product.GiaTruocKhiGiamGia = product.Gia + (product.Gia * percentDiscount) / 100;

                // Cập nhật sản phẩm trong cơ sở dữ liệu để lưu giữ giá trị GiaTruocKhiGiamGia
                _context.Entry(product).Property(x => x.GiaTruocKhiGiamGia).CurrentValue = product.GiaTruocKhiGiamGia;
            }
        }

        // Lưu các thay đổi vào cơ sở dữ liệu
        _context.SaveChanges();
    }

    public bool HasPromotion(int productId)
    {
        // Kiểm tra xem có bất kỳ chi tiết khuyến mãi nào cho sản phẩm không
        return _context.ChiTietKhuyenMais.Any(ctkm => ctkm.MaSanPham == productId);
    }

    public async Task<PromotionDetail> GetPromotionDetails(int productId)
    {
        // Lấy chi tiết khuyến mãi cho sản phẩm dựa trên productId
        var promotionDetail = await _context.ChiTietKhuyenMais
            .Where(p => p.MaSanPham == productId)
            .Select(p => new PromotionDetail
            {
                PhanTramKhuyenMai = p.MaKmNavigation.PhanTramKhuyenMai
                // Các trường khác của PromotionDetail nếu cần
            })
            .FirstOrDefaultAsync();

        return promotionDetail;
    }
}
