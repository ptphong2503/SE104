using System;
using System.Collections.Generic;

namespace SE104.Models;

public partial class TbAttributesPrice
{
    public int AttributeId { get; set; }

    public int? SizeId { get; set; }

    public int? ProductId { get; set; }

    public int? Price { get; set; }

    public bool Active { get; set; }

    public int? ColorId { get; set; }
}
