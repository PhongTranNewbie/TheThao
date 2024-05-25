using TheThao.Models;

namespace TheThao.ViewModel
{
    public class SanPhamGoiYViewModel
    {
        public int MaGoiY { get; set; }
        public string? TenGoiY { get; set; }
        public List<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}
