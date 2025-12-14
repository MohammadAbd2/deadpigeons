using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using api;
using api.services;
using tests.Containers;
using Xunit;

public abstract class TestBase : IClassFixture<PostgresFixture>
{
    protected readonly PostgresFixture _fixture;
    protected readonly IServiceProvider _services;

    protected TestBase(PostgresFixture fixture)
    {
        _fixture = fixture;

        var services = new ServiceCollection();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IJwtService>(sp =>
            new JwtService(new AppOptions { JWTSecret = "TEST_SECRET_1234567890123456" })
        );
        services.AddSingleton(_fixture.DbContext);

        _services = services.BuildServiceProvider();
    }

    protected T GetController<T>() where T : ControllerBase
    {
        return ActivatorUtilities.CreateInstance<T>(_services);
    }
}