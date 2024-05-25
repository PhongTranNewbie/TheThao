using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class Kho
{
    public int Serial { get; set; }

    public int? MaSanPham { get; set; }

    public string? TrangThai { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }

    public virtual QuanLySerial? QuanLySerial { get; set; }
}
