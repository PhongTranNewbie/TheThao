using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class ChiTietPhanTram
{
    public int Id { get; set; }

    public int MaSanPham { get; set; }

    public int? MaPhanTram { get; set; }

    public DateTime? NgayBatDau { get; set; }

    public DateTime? NgayKetThuc { get; set; }

    public virtual PhanTramGiaCa? MaPhanTramNavigation { get; set; }

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
