using System;
using System.Collections.Generic;

namespace SE104.Models;

public partial class TbProductCategory
{
    public int CatId { get; set; }

    public string CatName { get; set; }

    public string Description { get; set; }

    public int? Ordering { get; set; }

    public bool Published { get; set; }

    public string Image { get; set; }

    public string Alias { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? DateModified { get; set; }

    public string Title { get; set; }

    public int? AccountId { get; set; }

    public int? ParentId { get; set; }

    public int? RootId { get; set; }

    public int? Levels { get; set; }

    public virtual ICollection<TbProduct> TbProducts { get; set; } = new List<TbProduct>();
}
