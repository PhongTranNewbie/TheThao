using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class KhuyenMai
{
    public int Id { get; set; }

    public int? TenKm { get; set; }

    public int? PhanTramKhuyenMai { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<ChiTietKhuyenMai> ChiTietKhuyenMais { get; set; } = new List<ChiTietKhuyenMai>();

    public virtual User? User { get; set; }
}
