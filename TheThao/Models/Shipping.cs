using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class Shipping
{
    public int Id { get; set; }

    public string? DiaChi { get; set; }

    public DateTime? Ngay { get; set; }

    public decimal? TongTien { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual User? User { get; set; }
}
