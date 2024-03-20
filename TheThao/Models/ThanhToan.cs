using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class ThanhToan
{
    public int Id { get; set; }

    public DateTime? Ngay { get; set; }

    public string? PhuongPhapThanhToan { get; set; }

    public int? UserId { get; set; }

    public string? TrangThai { get; set; }

    public decimal? TongTien { get; set; }

    public int? MaDonHang { get; set; }

    public virtual DonHang? MaDonHangNavigation { get; set; }

    public virtual User? User { get; set; }
}
