using System;
using Common.Services.Validation;
using User.User.DTO;

namespace User.User.Validation
{
    public class UpdateUserDTOValidator : IValidator<UpdateUserDTO>
    {
        public ValidationResult Validate(UpdateUserDTO dto)
        {
            var validationResult = new ValidationResult();
            if (dto.Name.Length < 2)
            {
                return new ValidationResult("Name should be more 2 symbols");
            }

            return new ValidationResult();
        }
    }
}

