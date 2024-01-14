using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using SE104.Models;

namespace SE104.ModelsView
{
    public class ProductsView 
    {
        public TbProductCategory Category { get; set; }

        public List<TbProduct> lsProducts { get; set; }

        public List<TbProduct> LowtoHigh { get; set; }

        public List<TbProduct> HightoLow { get;set; }

    }
}
