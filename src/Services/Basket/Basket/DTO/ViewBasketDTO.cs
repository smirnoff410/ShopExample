using System;
using Basket.Product.DTO;

namespace Basket.Basket.DTO
{
    public class ViewBasketDTO
    {
        public int Id { get; set; }
        public ICollection<ViewProductDTO> Products { get; set; }
        public int TotalPrice { get; set; }
    }
}

