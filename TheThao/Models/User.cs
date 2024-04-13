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

    public virtual ICollection<BinhLuan> BinhLuans { get; set; } = new List<BinhLuan>();

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<KhuyenMai> KhuyenMais { get; set; } = new List<KhuyenMai>();

    public virtual ICollection<NhapHang> NhapHangs { get; set; } = new List<NhapHang>();

    public virtual ICollection<PhanTramGiaCa> PhanTramGiaCas { get; set; } = new List<PhanTramGiaCa>();

}
