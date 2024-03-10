using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheThao.Models;

public partial class ChiTietGioHang
{
    public int Id { get; set; }

    public int? IdSanPham { get; set; }

    public int? IdGioHang { get; set; }

    public int? Quantity { get; set; }
    [Column(TypeName = "decimal(12, 0)")]
    public decimal? Gia { get; set; }

    public virtual GioHang? IdGioHangNavigation { get; set; }

    public virtual SanPham? IdSanPhamNavigation { get; set; }
}
