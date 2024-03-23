namespace TheThao.Models
{
    public class ChiTietNhapHangViewModel
    {
        public int MaSanPham { get; set; } // Mã sản phẩm
        public int MaNhaCungCap { get; set; } // Mã nhà cung cấp
        public int SoLuongNhap { get; set; } // Số lượng nhập
        public decimal Gia { get; set; } // Giá
                                         // Các thuộc tính khác của Chi tiết nhập hàng nếu cần
        public decimal TongTien { get; set; } // Giá
    }
}
