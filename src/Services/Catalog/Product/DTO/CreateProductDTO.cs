using System;
namespace Catalog.Product.DTO
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int AvailableStock { get; set; }
    }
}

