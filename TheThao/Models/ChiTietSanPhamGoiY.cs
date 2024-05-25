using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class ChiTietSanPhamGoiY
{
    public int Id { get; set; }

    public int MaSanPham { get; set; }

    public int MaGoiY { get; set; }

    public virtual SanPhamGoiY MaGoiYNavigation { get; set; } = null!;

    public virtual SanPham MaSanPhamNavigation { get; set; } = null!;
}
