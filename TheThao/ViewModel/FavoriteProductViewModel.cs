using System;
using System.Collections.Generic;

namespace TheThao.ViewModels
{
    public class FavoriteProductViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ChiTiet { get; set; }
        public decimal? Gia { get; set; }
        public decimal? GiaTruocKhiGiamGia { get; set; }
        public string? HinhAnh { get; set; }
        public int? SoLuong { get; set; }
        public int? TheLoaiId { get; set; }
        public int? ThoiGianBaoHanh { get; set; }
        public int? SoLuongDaBan { get; set; }
        public string? TrangThai { get; set; }
        public bool IsFavorite { get; set; } // Trạng thái yêu thích của sản phẩm
    }
}
