using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Services.Validation;
using Microsoft.AspNetCore.Mvc;
using User.Services.DatabaseContext;
using User.User.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace User.User.Controller
{
    [Route("api/[controller]")]
    public class UserController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UserServiceDbContext _context;
        private readonly IValidator<CreateUserDTO> _createValidator;
        private readonly IValidator<UpdateUserDTO> _updateValidator;

        public UserController(
            UserServiceDbContext context,
            IValidator<CreateUserDTO> createValidator,
            IValidator<UpdateUserDTO> updateValidator)
        {
            _context = context;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult(_context.Users.ToList());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new OkObjectResult(_context.Users.Find(id));
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserDTO dto)
        {
            var validator = _createValidator.Validate(dto);
            if (!validator.Result)
                return new BadRequestObjectResult(validator.ErrorMessage);

            var user = new User.Entity.User
            {
                Name = dto.Name,
                Birthday = dto.Birthday
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return new OkObjectResult(user.Id);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UpdateUserDTO dto)
        {
            var validator = _updateValidator.Validate(dto);
            if (!validator.Result)
                return new BadRequestObjectResult(validator.ErrorMessage);

            var user = _context.Users.Find(id);

            user.Name = dto.Name;

            _context.SaveChanges();

            return new OkObjectResult(user.Id);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}

