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

    public async Task ApplyPromotionAsync(int productId, int percentDiscount)
    {
        // Lấy sản phẩm từ cơ sở dữ liệu
        var product = await _context.SanPhams.FindAsync(productId);
        if (product != null)
        {
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
                    // Áp dụng giảm giá cho sản phẩm
                    product.Gia -= (product.Gia * percentDiscount) / 100;
                    _context.SanPhams.Update(product);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
