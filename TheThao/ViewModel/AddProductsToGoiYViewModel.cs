using TheThao.Models;

namespace TheThao.ViewModel
{
    public class AddProductsToGoiYViewModel
    {
        public int MaGoiY { get; set; }
        public string? TenGoiY { get; set; }  // Adding this for displaying the recommendation name
        public List<SanPham> SanPhams { get; set; } = new List<SanPham>();
        public List<int> SelectedSanPhams { get; set; } = new List<int>();

    }
}