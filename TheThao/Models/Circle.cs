using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class Circle
{
    public int Id { get; set; }

    public string? HinhAnh { get; set; }

    public bool? IsVisible { get; set; }
}
