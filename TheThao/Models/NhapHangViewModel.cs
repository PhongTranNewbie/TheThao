using System.ComponentModel.DataAnnotations;

namespace TheThao.Models
{
    public class NhapHangViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; } // ID của người dùng thực hiện nhập hàng
        [Display(Name = "Ngày Nhập")]
        public DateTime? NgayNhap { get; set; }
        public List<ChiTietNhapHangViewModel> ChiTietNhapHangs { get; set; } // Danh sách các chi tiết nhập hàng

        // Các thuộc tính khác của Nhập hàng nếu cần
    }

   
}
