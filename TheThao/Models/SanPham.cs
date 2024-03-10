using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class SanPham
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ChiTiet { get; set; }

    public int? Gia { get; set; }

    public string? HinhAnh { get; set; }

    public int? SoLuong { get; set; }

    public int? TheLoaiId { get; set; }

    public int? ThoiGianBaoHanh { get; set; }
    public int? SoluongDaBan {  get; set; }
    public string? TrangThai {  get; set; }

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; } = new List<ChiTietGioHang>();

    public virtual ICollection<ChiTietHinhAnh> ChiTietHinhAnhs { get; } = new List<ChiTietHinhAnh>();

    public virtual ICollection<ChiTietNhapHang> ChiTietNhapHangs { get; } = new List<ChiTietNhapHang>();

    public virtual LoaiSanPham? TheLoai { get; set; }
}
