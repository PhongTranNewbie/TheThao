using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class PhanTramGiaCa
{
    public int Id { get; set; }

    public int PhanTram { get; set; }

    public virtual ICollection<ChiTietPhanTram> ChiTietPhanTrams { get; set; } = new List<ChiTietPhanTram>();
}
