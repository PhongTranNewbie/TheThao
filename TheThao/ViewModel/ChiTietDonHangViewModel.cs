using TheThao.Models;

namespace TheThao.ViewModels
{
    public class ChiTietDonHangViewModel
    {
        public DonHang DonHang { get; set; }
        public List<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public decimal TongTien { get; set; } // Thuộc tính để lưu tổng tiền
    }
}
