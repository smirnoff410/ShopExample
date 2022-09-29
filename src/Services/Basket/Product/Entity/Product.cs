using System;
namespace Basket.Product.Entity
{
    using Basket.Entity;
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public ICollection<Basket> Baskets { get; set; }
    }
}

