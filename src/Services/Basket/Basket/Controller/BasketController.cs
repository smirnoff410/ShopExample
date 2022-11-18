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
        public IActionResult Get(int id)
        {
            var baskets = _context.Baskets.Include(x => x.Products);
            var str = baskets.ToQueryString();
            var res = baskets.ToList();
            return new OkObjectResult(_context.Baskets.Find(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateBasketDTO dto)
        {
            var user = _context.Users.ToList();
            var basket = new Basket.Entity.Basket
            {
                UserId = dto.UserId
            };

            _context.Baskets.Add(basket);
            _context.SaveChanges();

            return new OkObjectResult(basket.Id);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateBasketDTO dto)
        {
            var basket = _context.Baskets.Find(id);
            var products = _context.Products.Where(x => dto.Products.Select(c => c.Id).Contains(x.Id)).ToList();
            products.ForEach(x =>
            {
                var count = dto.Products.FirstOrDefault(c => c.Id == x.Id).Count;
                x.Count = count;
            });
            basket.Products = products;

            _context.SaveChanges();

            return new OkObjectResult(basket.Id);
        }

        [HttpPost("{id}/order")]
        public IActionResult MakeOrder(int id)
        {
            var basket = _context.Baskets.Include(x => x.Products).FirstOrDefault(x => x.Id == id);

            foreach(var product in basket.Products)
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

            return new OkObjectResult(true);
        }
    }
}

