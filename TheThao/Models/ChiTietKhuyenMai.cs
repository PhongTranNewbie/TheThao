using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class ChiTietKhuyenMai
{
    public int Id { get; set; }

    public int? MaSanPham { get; set; }

    public int? MaKm { get; set; }

    public DateTime? NgayBatDau { get; set; }

    public DateTime? NgayKetThuc { get; set; }

    public virtual KhuyenMai? MaKmNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
