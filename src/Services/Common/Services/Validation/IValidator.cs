using System;
namespace Common.Services.Validation
{
    public interface IValidator<T>
    {
        ValidationResult Validate(T dto);
    }
}

