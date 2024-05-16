using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class BaoHanh
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? MaSerial { get; set; }

    public DateTime? NgayBaoHanh { get; set; }

    public DateTime? NgayKetThucBaoHanh { get; set; }

    public string? TrangThai { get; set; }


    public string? LyDo { get; set; }

    public virtual QuanLySerial? MaSerialNavigation { get; set; }
}
