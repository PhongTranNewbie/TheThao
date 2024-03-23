using System.ComponentModel.DataAnnotations;

namespace TheThao.ViewModel
{
    public class ChiTietNhapHangViewModel
    {
        public int MaSanPham { get; set; }
        public int SoLuong { get; set; }
        [Display(Name = "Mã Nhà Cung Cấp")]
        public int? MaNhaCungCap { get; set; }
        public decimal Gia { get; set; }
    }
}
