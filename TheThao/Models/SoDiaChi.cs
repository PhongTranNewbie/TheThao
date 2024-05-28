using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class SoDiaChi
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? ThanhPho { get; set; }

    public string? QuanHuyen { get; set; }

    public string? Xa { get; set; }

    public virtual User? User { get; set; }
}
