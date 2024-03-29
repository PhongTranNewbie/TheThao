using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class DonHang
{
    public string Id { get; set; } = null!;

    public DateTime? NgayTao { get; set; }

    public DateTime? Ngay { get; set; }

    public int? TrangThai { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual User? User { get; set; }
}
