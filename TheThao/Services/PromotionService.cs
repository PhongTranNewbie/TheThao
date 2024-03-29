using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheThao.Migrations;
using TheThao.Models;
using TheThao.Services;

public class PromotionService : IPromotionService
{
    private readonly TheThaoContext _context;

    public PromotionService(TheThaoContext context)
    {
        _context = context;
    }

    public async Task ApplyPromotionAsync()
    {
        // Lấy tất cả sản phẩm từ cơ sở dữ liệu
        var products = await _context.SanPhams.ToListAsync();

        foreach (var product in products)
        {
            // Lấy mã sản phẩm của sản phẩm hiện tại
            var productId = product.Id;

            // Lấy chi tiết khuyến mãi gần nhất cho sản phẩm
            var latestPromotion = await _context.ChiTietKhuyenMais
                .Where(c => c.MaSanPham == productId && c.NgayBatDau <= DateTime.Now)
                .OrderByDescending(c => c.NgayBatDau)
                .FirstOrDefaultAsync();

            if (latestPromotion != null)
            {
                // Kiểm tra xem ngày kết thúc khuyến mãi đã hết chưa
                if (latestPromotion.NgayKetThuc >= DateTime.Now)
                {
                    // Tính toán giảm giá dựa trên phần trăm khuyến mãi của chi tiết khuyến mãi
                    int percentDiscount = latestPromotion.MaKmNavigation.PhanTramKhuyenMai;

                    // Áp dụng giảm giá cho sản phẩm
                    product.Gia -= (product.Gia * percentDiscount) / 100;
                }
            }
        }

        // Lưu các thay đổi vào cơ sở dữ liệu
        await _context.SaveChangesAsync();
    }
    public bool HasPromotion(int productId)
    {
        // Kiểm tra xem có bất kỳ chi tiết khuyến mãi nào cho sản phẩm không
        return _context.ChiTietKhuyenMais.Any(ctkm => ctkm.MaSanPham == productId);
    }
}
