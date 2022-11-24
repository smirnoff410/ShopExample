using System;
using User.User.DTO;
using User.User.Validation;

namespace UnitTests.User
{
    public class UpdateUserValidationTest
    {
        [Fact]
        public void UpdateUserValidationCorrectTest()
        {
            var dto = new UpdateUserDTO
            {
                Name = "userName"
            };

            var validator = new UpdateUserDTOValidator();
            var result = validator.Validate(dto);

            Assert.True(result.Result);
        }

        [Fact]
        public void CreateUserValidationNameFailedTest()
        {
            var dto = new UpdateUserDTO
            {
                Name = "Я"
            };

            var validator = new UpdateUserDTOValidator();
            var result = validator.Validate(dto);

            Assert.False(result.Result);
            Assert.Equal("Name should be more 2 symbols", result.ErrorMessage);
        }
    }
}

