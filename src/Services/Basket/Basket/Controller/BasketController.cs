using System;
using Basket.Basket.DTO;
using Basket.Services.DatabaseContext;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Basket.Controller
{
    using Common.Models.Product;
    using Common.Services.MessageQueue;
    using Microsoft.EntityFrameworkCore;
    using Product.Entity;
    using Product.DTO;
    [Route("api/[controller]")]
    public class BasketController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly BasketServiceDbContext _context;
        private readonly IMessageQueue _messageQueue;

        public BasketController(BasketServiceDbContext context, IMessageQueue messageQueue)
        {
            _context = context;
            _messageQueue = messageQueue;
        }

        [HttpGet("{id}")]
        public IActionResult GetByUserId(int id)
        {
            var basket = _context.Baskets.Include(x => x.Products).FirstOrDefault(x => x.UserId == id);
            if (basket == null)
                return new OkResult();
            var dto = new ViewBasketDTO
            {
                Id = basket.Id,
                Products = basket.Products.Select(x => new ViewProductDTO
                {
                    Name = x.Name,
                    Count = x.Count,
                    Price = x.Price,
                    ImageUrl = x.ImageUrl
                }).ToList(),
                TotalPrice = basket.Products.Sum(x => x.Price * x.Count)
            };
            return new OkObjectResult(dto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateBasketDTO dto)
        {
            var basket = _context.Baskets.Include(x => x.Products).FirstOrDefault(x => x.UserId == id);

            if(basket == null)
            {
                basket = new Entity.Basket { UserId = id };
                _context.Baskets.Add(basket);
            }

            var basketProduct = basket.Products?.FirstOrDefault(x => x.ProductId == dto.Product.Id);
            if (basketProduct != null)
            {
                basketProduct.Count += dto.Product.Count;
            }
            else
            {
                var product = _context.Products.Find(dto.Product.Id);
                basket.Products = new List<BasketProduct.Entity.BasketProduct>
                {
                    new BasketProduct.Entity.BasketProduct
                    {
                        Name = product.Name,
                        Price = product.Price,
                        ImageUrl = product.ImageUrl,
                        ProductId = product.Id,
                        Count = dto.Product.Count
                    }
                };
            }

            _context.SaveChanges();

            return new OkObjectResult(basket.Id);
        }

        [HttpPost("{id}/order")]
        public IActionResult MakeOrder(int id)
        {
            var basket = _context.Baskets.Include(x => x.Products).FirstOrDefault(x => x.UserId == id);

            foreach (var product in basket.Products)
            {
                //Отправить запрос на сервис Catalog для проверки на уменьшение кол-ва каждого продукта
                var response = _messageQueue.SendMessageRpc<CheckProductIntegrationMessage, bool>(
                    "check_product_count", new CheckProductIntegrationMessage { ProductId = product.Id, ProductCount = product.Count });
                //Если получили хоть один отказ, то весь процесс можно отменять
                if (!response)
                {
                    return new BadRequestObjectResult($"К сожалению у нас нет столько товара {product.Count} с названием {product.Name}");
                }
            }
            _messageQueue.Publish<DecreaseAvailableCountProductMessage>("change_count_product", new DecreaseAvailableCountProductMessage
            {
                Products = basket.Products
                .Select(x => new CheckProductIntegrationMessage { ProductId = x.Id, ProductCount = x.Count })
                .ToList()
            });

            _context.Baskets.Remove(basket);
            _context.SaveChanges();

            return new OkObjectResult(true);
        }
    }
}

