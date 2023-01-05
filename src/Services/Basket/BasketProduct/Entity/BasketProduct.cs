using System;
namespace Basket.BasketProduct.Entity
{
    using Basket.Entity;

    public class BasketProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
        public Basket Basket { get; set; }
        public int BasketId { get; set; }
    }
}

