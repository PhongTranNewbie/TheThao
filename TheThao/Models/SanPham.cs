using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class SanPham
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

    public int? NamBaoHanh { get; set; }

    public decimal? GiaSi { get; set; }

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<ChiTietHinhAnh> ChiTietHinhAnhs { get; set; } = new List<ChiTietHinhAnh>();

    public virtual ICollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; } = new List<ChiTietKhuyenMai>();

    public virtual ICollection<ChiTietNhapHang> ChiTietNhapHangs { get; set; } = new List<ChiTietNhapHang>();

    public virtual ICollection<ChiTietPhanTram> ChiTietPhanTrams { get; set; } = new List<ChiTietPhanTram>();

    public virtual ICollection<ChiTietSanPhamGoiY> ChiTietSanPhamGoiYs { get; set; } = new List<ChiTietSanPhamGoiY>();

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Kho> Khos { get; set; } = new List<Kho>();

    public virtual LoaiSanPham? TheLoai { get; set; }
}