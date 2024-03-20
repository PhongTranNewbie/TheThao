using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class ChiTietNhapHang
{
    public int Id { get; set; }

    public int? MaSanPham { get; set; }

    public int? MaNhapHang { get; set; }

    public int? MaNhaCungCap { get; set; }

    public int? SoLuongNhap { get; set; }

    public decimal? Gia { get; set; }

    public decimal? TongTien { get; set; }

    public virtual NhaCungCap? MaNhaCungCapNavigation { get; set; }

    public virtual NhapHang? MaNhapHangNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
