using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class ChiTietDonHang
{
    public int Id { get; set; }

    public int? MaSanPham { get; set; }

    public string? MaDonHang { get; set; }

    public decimal? Gia { get; set; }

    public int? SoLuong { get; set; }

    public virtual DonHang? MaDonHangNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
