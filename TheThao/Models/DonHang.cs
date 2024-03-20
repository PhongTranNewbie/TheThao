using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class DonHang
{
    public int Id { get; set; }

    public DateTime? Ngay { get; set; }

    public string? TrangThai { get; set; }

    public int? UserId { get; set; }

    public decimal? TongTien { get; set; }

    public int? ShippingId { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual Shipping? Shipping { get; set; }

    public virtual ThanhToan? ThanhToan { get; set; }

    public virtual User? User { get; set; }
}
