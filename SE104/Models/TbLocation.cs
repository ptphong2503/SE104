using System;
using System.Collections.Generic;

namespace SE104.Models;

public partial class TbLocation
{
    public int LocationId { get; set; }

    public string Name { get; set; }

    public int? Parent { get; set; }

    public int? Levels { get; set; }

    public string Slug { get; set; }

    public string NameWithType { get; set; }

    public string Type { get; set; }
}
