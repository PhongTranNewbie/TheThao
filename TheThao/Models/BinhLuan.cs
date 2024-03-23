using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class BinhLuan
{
    public int Id { get; set; }

    public int? MaSanPham { get; set; }

    public int? UserId { get; set; }

    public string? BinhLuan1 { get; set; }

    public DateTime? NgayTao { get; set; }

    public DateTime? NgayCuoiCapNhat { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }

    public virtual User? User { get; set; }
}
