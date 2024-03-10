using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class NhaCungCap
{
    public int Id { get; set; }

    public string? TenNhaCungCap { get; set; }

    public string? DiaChi { get; set; }

    public string? Sdt { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<ChiTietNhapHang> ChiTietNhapHangs { get; } = new List<ChiTietNhapHang>();
}
