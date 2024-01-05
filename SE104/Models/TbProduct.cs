using System;
using System.Collections.Generic;

namespace SE104.Models;

public partial class TbProduct
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ShortDesc { get; set; }

    public string Description { get; set; }

    public int? CatId { get; set; }

    public int? Price { get; set; }

    public int? Discount { get; set; }

    public string Image { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? DateModified { get; set; }

    public bool BestSellers { get; set; }

    public bool IsNew { get; set; }

    public bool Active { get; set; }

    public string Tags { get; set; }

    public string Alias { get; set; }

    public int? UnitsInStock { get; set; }

    public string Title { get; set; }

    public int? SalePrice { get; set; }

    public virtual TbProductCategory Cat { get; set; }

    public virtual ICollection<TbProductImage> ProductImage { get; set; } = new List<TbProductImage>();

    public virtual ICollection<TbAttributesPrice> TbAttributesPrices { get; set; } = new List<TbAttributesPrice>();

    public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; } = new List<TbOrderDetail>();

}
