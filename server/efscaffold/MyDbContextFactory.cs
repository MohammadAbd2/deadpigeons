using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace efscaffold;

public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
{
    public MyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=ep-soft-resonance-ad7u4o8b-pooler.c-2.us-east-1.aws.neon.tech;" +
            "Database=deadpigeons;" +
            "Username=neondb_owner;" +
            "Password=npg_mgTa1eJtVC7o;" +
            "SSL Mode=Require;" +
            "Trust Server Certificate=true;" +
            "Channel Binding=Require;",
            npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(5)
        );

        return new MyDbContext(optionsBuilder.Options);
    }
}