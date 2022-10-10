using System;
using Common.Services.Validation;
using User.User.DTO;

namespace User.User.Validation
{
    public class CreateUserDTOValidator : IValidator<CreateUserDTO>
    {
        public ValidationResult Validate(CreateUserDTO dto)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            var eigthYears = new DateOnly(18, 1, 1);
            if(currentDate.DayNumber - dto.Birthday.DayNumber <= eigthYears.DayNumber)
            {
                return new ValidationResult("You should be more 18 years old");
            }

            if(dto.Name.Length < 2)
            {
                return new ValidationResult("Name should be more 2 symbols");
            }

            return new ValidationResult();
        }
    }
}

