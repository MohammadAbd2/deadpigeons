using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace efscaffold;

public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config["AppOptions:DbConnectionString"];

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'AppOptions:DbConnectionString' not found"
            );
        }

        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

        optionsBuilder.UseNpgsql(
            connectionString,
            o => o.EnableRetryOnFailure()
        );

        return new MyDbContext(optionsBuilder.Options);
    }
}