using System.Linq;

namespace TheThao.Models
{
    public class ProductManager
    {
        private readonly TheThaoContext _context;

        public ProductManager(TheThaoContext context)
        {
            _context = context;
        }

        public void UpdateProductDetails(int productId)
        {
            // Lấy thông tin sản phẩm từ cơ sở dữ liệu
            var product = _context.SanPhams.Find(productId);
            if (product == null)
            {
                // Xử lý khi sản phẩm không tồn tại
                return;
            }

            // Lấy danh sách chi tiết nhập hàng cho sản phẩm
            var chiTietNhapHangs = _context.ChiTietNhapHangs
                .Where(ct => ct.MaSanPham == productId)
                .ToList();

            if (chiTietNhapHangs.Any())
            {
                // Tính tổng số lượng nhập hàng
                int tongSoLuongNhapHang = (int)chiTietNhapHangs.Sum(ct => ct.SoLuongNhap);

                // Tính giá mới của sản phẩm (giá từ chi tiết nhập hàng + 23%)
                decimal giaMoi = (decimal)(chiTietNhapHangs.Sum(ct => ct.Gia) * 1.23m);

                // Cập nhật số lượng và giá mới của sản phẩm
                product.SoLuong = tongSoLuongNhapHang;
                product.Gia = (int)giaMoi;

                // Lưu các thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();
            }
        }
    }
}
