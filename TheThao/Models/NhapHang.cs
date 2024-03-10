using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class NhapHang
{
    public int Id { get; set; }

    public DateTime NgayNhapHang { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<ChiTietNhapHang> ChiTietNhapHangs { get; } = new List<ChiTietNhapHang>();

    public virtual User? User { get; set; }
}
