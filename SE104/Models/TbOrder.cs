using System;
using System.Collections.Generic;

namespace SE104.Models;

public partial class TbOrder
{
    public int OrderId { get; set; }

    public string Code { get; set; }

    public int? CustomerId { get; set; }

    public string Receiver { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? ShipDate { get; set; }

    public int TransactStatusId { get; set; }

    public bool Deleted { get; set; }

    public bool Paid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int TotalMoney { get; set; }

    public int? PaymentId { get; set; }

    public string Note { get; set; }

    public string Address { get; set; }

    public int? LocationId { get; set; }

    public int? District { get; set; }

    public int? Ward { get; set; }

    public int? Quantity { get; set; }

    public string PhoneNumber { get; set; }

    public virtual TbCustomer  Customer { get; set; }

    public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; } = new List<TbOrderDetail>();

    public virtual TbTransactStatus TransactStatus { get; set; } = null!;
}
