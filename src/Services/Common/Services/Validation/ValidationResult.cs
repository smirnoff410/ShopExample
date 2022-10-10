using System;
namespace Common.Services.Validation
{
    public class ValidationResult
    {
        public bool Result { get; }
        public string ErrorMessage { get; }

        public ValidationResult()
        {
            Result = true;
        }

        public ValidationResult(string errorMessage)
        {
            Result = false;
            ErrorMessage = errorMessage;
        }
    }
}

