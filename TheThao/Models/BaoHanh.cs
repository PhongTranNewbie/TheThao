using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class BaoHanh
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? MaSanPham { get; set; }

    public DateTime? NgayBaoHanh { get; set; }

    public DateTime? NgayKetThucBaoHanh { get; set; }

    public string? TrangThai { get; set; }

    public int? MaDonHang { get; set; }

    public virtual ChiTietDonHang? MaDonHangNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }

    public virtual User? User { get; set; }
}
