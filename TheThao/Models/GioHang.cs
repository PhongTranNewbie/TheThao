using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class GioHang
{
    public int Id { get; set; }

    public string? MaGiaoDichUser { get; set; }

    public string? MaGiaoDichMerchant { get; set; }

    public int? MaDonHang { get; set; }

    public DateTime? NgayTao { get; set; }

    public int? UserId { get; set; }


    public virtual DonHang? MaDonHangNavigation { get; set; }

    public virtual User? User { get; set; }
}
