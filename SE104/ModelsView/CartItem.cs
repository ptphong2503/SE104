using Microsoft.AspNetCore.Mvc;
using SE104.Models;

namespace SE104.ModelsView
{
    public class CartItem 
    {
        public TbAttributesPrice attributesPrice { get; set; }
        public int quantity { get; set; }
        public int TotalPrice { get; set; }

    }
}
