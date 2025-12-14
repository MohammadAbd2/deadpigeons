using Xunit;
using Microsoft.AspNetCore.Mvc;
using api.Controllers;
using api.services;
using efscaffold.Models;
using tests.Containers;

namespace tests.Controllers;

public class AuthControllerTests : TestBase
{
    public AuthControllerTests(PostgresFixture fixture) : base(fixture) {}

    [Fact]
    public void Login_ShouldFail_WhenEmailEmpty()
    {
        var controller = GetController<AuthController>();

        var result = controller.Login(new AuthController.LoginRequest
        {
            Email = "",
            Password = "x"
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }
    

    [Fact]
    public void Login_ShouldReturnToken_ForValidUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = "test@test.com",
            Password = new PasswordService().HashPassword("password")
        };
        _fixture.DbContext.Users.Add(user);
        _fixture.DbContext.SaveChanges();

        var controller = GetController<AuthController>();

        // Act
        var result = controller.Login(new AuthController.LoginRequest
        {
            Email = user.Email,
            Password = "password"
        });

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthController.LoginResponse>(ok.Value);
        Assert.NotNull(response.Token);
        Assert.Equal("user", response.Role);
    }
}
