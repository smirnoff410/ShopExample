using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.User;
using Common.Services.Command;
using Common.Services.MessageQueue;
using Common.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using User.Services.DatabaseContext;
using User.User.Command;
using User.User.DTO;
using User.User.Validation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User.User.Controller
{
    [Route("api/[controller]")]
    public class UserController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UserServiceDbContext _context;

        public UserController(
            IServiceProvider serviceProvider,
            UserServiceDbContext context)
        {
            _serviceProvider = serviceProvider;
            _context = context;
        }

        // GET: api/user
        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users.Select(x => new ViewUserDTO
            {
                Id = x.Id,
                Name = x.Name,
                Birthday = x.Birthday
            }).ToList();
            return new OkObjectResult(users);
        }

        // GET api/product/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _context.Users.Find(id);
            var dto = new ViewUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Birthday = user.Birthday
            };
            return new OkObjectResult(dto);
        }

        // POST api/product
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserDTO dto)
        {
            //Паттерн: Фабричный метод
            var validator = new CreateUserDTOValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.Result)
            {
                return new BadRequestObjectResult(validationResult.ErrorMessage);
            }
            //Паттерн: Команда
            var mainCommand = new CreateUserCommand(_serviceProvider, dto);
            var integrationCommand = new CreateUserIntegrationCommand(_serviceProvider);

            var invoker = new CommandInvoker(mainCommand, integrationCommand);
            var response = invoker.Invoke();

            return response;
        }

        // PUT api/product/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateUserDTO dto)
        {
            var validator = new UpdateUserDTOValidator();
            var validationResult = validator.Validate(dto);
            if (!validationResult.Result)
            {
                return new BadRequestObjectResult(validationResult.ErrorMessage);
            }
            var mainCommand = new UpdateUserCommand(_serviceProvider, dto, id);

            var invoker = new CommandInvoker(mainCommand);
            var response = invoker.Invoke();

            return response;
        }

        // DELETE api/product/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var command = new DeleteUserCommand(_serviceProvider, id);
            var invoker = new CommandInvoker(command);
            invoker.Invoke();
        }
    }
}

