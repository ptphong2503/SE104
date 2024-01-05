using System;
using System.Collections.Generic;

namespace SE104.Models;

public partial class TbOrderDetail
{
    public int OrderDetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? OrderNumber { get; set; }

    public int? Quantity { get; set; }

    public int? Discount { get; set; }

    public int? TotalMoney { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? Price { get; set; }

    public string ColorName { get; set; }

    public string SizeValue { get; set; }

    public string Image { get; set; }

    public virtual TbOrder Order { get; set; }
}
