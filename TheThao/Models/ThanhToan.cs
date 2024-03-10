using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheThao.Models;

public partial class ThanhToan
{
    public int Id { get; set; }

    public DateTime? Ngay { get; set; }

    public string? PhuongPhapThanhToan { get; set; }

    public int? MaGioHang { get; set; }

    public int? UserId { get; set; }

    public string? TrangThai { get; set; }
    [Column(TypeName = "decimal(12, 0)")]
    public decimal? TongTien { get; set; }

    public virtual GioHang? MaGioHangNavigation { get; set; }

    public virtual User? User { get; set; }
}
