using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace VisitorManagementSystem.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>();

        builder.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"), b => b.MigrationsAssembly("VisitorManagementSystem"));

        return new RepositoryContext(builder.Options);
    }
}