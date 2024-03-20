using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class LoaiSanPham
{
    public int Id { get; set; }

    public string? TenLoaiSanPham { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
