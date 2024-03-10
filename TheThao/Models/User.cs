using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Ten { get; set; }

    public DateTime? NgayThangNamSinh { get; set; }

    public string? DiaChi { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; } = new List<DonHang>();

    public virtual ICollection<GioHang> GioHangs { get; } = new List<GioHang>();

    public virtual ICollection<NhapHang> NhapHangs { get; } = new List<NhapHang>();

    public virtual ICollection<Shipping> Shippings { get; } = new List<Shipping>();

    public virtual ICollection<ThanhToan> ThanhToans { get; } = new List<ThanhToan>();
}
