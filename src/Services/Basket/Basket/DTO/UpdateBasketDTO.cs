using System;
namespace Basket.Basket.DTO
{
    public class UpdateBasketDTO
    {
        public ICollection<UpdateProductDTO> Products { get; set; }
    }
}

