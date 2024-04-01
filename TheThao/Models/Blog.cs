using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string? NoiDung { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
