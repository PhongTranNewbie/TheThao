using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheThao.Models;

public partial class DonHang
{
    public int Id { get; set; }

    public DateTime? Ngay { get; set; }

    public string? TrangThai { get; set; }

    public int? UserId { get; set; }
    [Column(TypeName = "decimal(12, 0)")]
    public decimal? TongTien { get; set; }

    public int? ShippingId { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<GioHang> GioHangs { get; } = new List<GioHang>();

    public virtual Shipping? Shipping { get; set; }

    public virtual User? User { get; set; }
}
