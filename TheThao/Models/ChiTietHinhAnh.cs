using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class ChiTietHinhAnh
{
    public int Id { get; set; }

    public int? MaSanPham { get; set; }

    public string? HinhAnh { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}
