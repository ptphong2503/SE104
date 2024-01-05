using System;
using System.Collections.Generic;

namespace SE104.Models;

public partial class TbProductImage
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Image { get; set; }

    public bool IsDefault { get; set; }
}
