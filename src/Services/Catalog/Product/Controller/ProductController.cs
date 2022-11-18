using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Product.Command;
using Catalog.Product.DTO;
using Catalog.Services.DatabaseContext;
using Common.Services.Command;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.Product.Controller
{
    [Route("api/[controller]")]
    public class ProductController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IServiceProvider _provider;
        private readonly CatalogServiceDbContext _context;

        public ProductController(IServiceProvider provider, CatalogServiceDbContext context)
        {
            _provider = provider;
            _context = context;
        }
        // GET: api/product
        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult(_context.Products.ToList());
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new OkObjectResult(_context.Products.Find(id));
        }

        // POST api/product
        [HttpPost]
        public IActionResult Post([FromBody] CreateProductDTO dto)
        {
            var mainCommand = new CreateProductCommand(_provider, dto);
            var integrationCommand = new CreateProductIntegrationCommand(_provider);

            var invoker = new CommandInvoker(mainCommand, integrationCommand);
            var response = invoker.Invoke();
            return response;
        }

        // POST api/product/5/change_available
        [HttpPost("{id}/change_available")]
        public IActionResult ChangeAvailable(int id, ChangeAvailableDTO dto)
        {
            var mainCommand = new ChangeAvailableCommand(_provider, dto, id);

            var invoker = new CommandInvoker(mainCommand);
            var response = invoker.Invoke();
            return response;
        }
    }
}

