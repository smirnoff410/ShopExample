using System;
namespace Basket.Basket.Entity
{
    using User.Entity;
    using Product.Entity;
    public class Basket
    {
        public int Id { get; set; }
        public User User { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}