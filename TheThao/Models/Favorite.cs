using System;
using System.Collections.Generic;

namespace TheThao.Models;

public partial class Favorite
{
    public int ProductId { get; set; }

    public int UserId { get; set; }

    public DateTime? FavoriteDate { get; set; }

    public virtual SanPham Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
