using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class QuanLySerial
{
    public int SerialId { get; set; }

    public int? MaChiTietDonHang { get; set; }

    public virtual BaoHanh? BaoHanh { get; set; }

    public virtual ChiTietDonHang? MaChiTietDonHangNavigation { get; set; }

    public virtual Kho Serial { get; set; } = null!;
}
