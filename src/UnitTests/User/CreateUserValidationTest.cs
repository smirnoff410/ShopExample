using User.User.DTO;
using User.User.Validation;

namespace UnitTests.User;

public class CreateUserValidationTest
{
    [Fact]
    public void CreateUserValidationCorrectTest()
    {
        var dto = new CreateUserDTO
        {
            Name = "userName",
            Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18))
        };

        var validator = new CreateUserDTOValidator();
        var result = validator.Validate(dto);

        Assert.True(result.Result);
    }

    [Fact]
    public void CreateUserValidationNameFailedTest()
    {
        var dto = new CreateUserDTO
        {
            Name = "Я",
            Birthday = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-18))
        };

        var validator = new CreateUserDTOValidator();
        var result = validator.Validate(dto);

        Assert.False(result.Result);
        Assert.Equal("Name should be more 2 symbols", result.ErrorMessage);
    }

    [Fact]
    public void CreateUserValidationBirthdayFailedTest()
    {
        var dto = new CreateUserDTO
        {
            Name = "userName",
            Birthday = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        var validator = new CreateUserDTOValidator();
        var result = validator.Validate(dto);

        Assert.False(result.Result);
        Assert.Equal("You should be more 18 years old", result.ErrorMessage);
    }
}
