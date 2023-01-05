using System;
namespace Basket.Basket.Entity
{
    using User.Entity;
    using Product.Entity;
    using BasketProduct.Entity;

    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<BasketProduct> Products { get; set; }
    }
}