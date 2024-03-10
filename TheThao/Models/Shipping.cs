using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheThao.Models;

public partial class Shipping
{
    public int Id { get; set; }

    public string? DiaChi { get; set; }

    public DateTime? Ngay { get; set; }
    [Column(TypeName = "decimal(12, 0)")]

    public decimal? TongTien { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; } = new List<DonHang>();

    public virtual User? User { get; set; }
}
