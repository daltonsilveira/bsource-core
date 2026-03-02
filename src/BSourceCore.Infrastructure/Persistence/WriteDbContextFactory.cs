using BSourceCore.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BSourceCore.Infrastructure.Persistence;

public class WriteDbContextFactory : IDesignTimeDbContextFactory<WriteDbContext>
{
    public WriteDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "BSourceCore.API"))
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<WriteDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new WriteDbContext(optionsBuilder.Options, new DesignTimeUserContext(), new DesignTimeTenantContext());
    }

    private class DesignTimeTenantContext : ITenantContext
    {
        public Guid? TenantId => null;
    }

    private class DesignTimeUserContext : IUserContext
    {
        public Guid? TenantId => null;
        public Guid? UserId => null;
        public string Email => string.Empty;
        public string IpAddress => string.Empty;
        public string UserAgent => string.Empty;
        public bool IsAuthenticated => false;
    }
}
