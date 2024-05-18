using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class SanPhamGoiY
{
    public int Id { get; set; }

    public string? Ten { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<ChiTietSanPhamGoiY> ChiTietSanPhamGoiYs { get; set; } = new List<ChiTietSanPhamGoiY>();
}
