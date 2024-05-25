using TheThao.Models;
namespace TheThao.Service
{
    public class addToStock
    {
        private readonly TheThaoContext _context; // Your DbContext instance

        public addToStock(TheThaoContext context)
        {
            _context = context;
        }

        public void AddSerialsForAllProducts()
        {
            // Truy xuất tất cả các sản phẩm từ bảng SanPhams
            var products = _context.SanPhams.ToList();

            foreach (var product in products)
            {
                // Tạo serial numbers cho mỗi sản phẩm
                AddSerialsToKho(product.Id, product.SoLuong ?? 0);
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();
        }

        private void AddSerialsToKho(int productId, int quantity)
        {
            // Thêm số lượng serial numbers tương ứng vào bảng Kho
            for (int i = 0; i < quantity; i++)
            {
                var serial = new Kho
                {
                    MaSanPham = productId,
                    TrangThai = "Active"
                };

                _context.Khos.Add(serial);
            }
        }

    }
}
