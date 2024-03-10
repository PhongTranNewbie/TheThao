using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TheThao.Models;

namespace TheThao.ViewModels
{
    public class NhapHangViewModel
    {
        public NhapHangViewModel()
        {
            ChiTietNhapHangs = new List<ChiTietNhapHangViewModel>(); // Khởi tạo danh sách rỗng
        }

        [Display(Name = "Ngày Nhập")]
        public DateTime? NgayNhap { get; set; }

        [Display(Name = "Mã Nhà Cung Cấp")]
        public int? MaNhaCungCap { get; set; }

        [Display(Name = "User Id")]
        public int? UserId { get; set; }

        [Display(Name = "Chi Tiết Nhập Hàng")]
        public List<ChiTietNhapHangViewModel> ChiTietNhapHangs { get; set; }
    }

    public class ChiTietNhapHangViewModel
    {
        [Display(Name = "Mã Sản Phẩm")]
        public int MaSanPham { get; set; }

        [Display(Name = "Số Lượng")]
        public int SoLuong { get; set; }

        [Display(Name = "Giá")]
        public decimal Gia { get; set; }
    }
}
