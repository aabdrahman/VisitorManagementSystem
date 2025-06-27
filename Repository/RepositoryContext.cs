using Entities.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class RepositoryContext : IdentityDbContext<User, Role, string>
{
    public DbSet<VisitDetail> VisitDetails { get; set; }
    public DbSet<Visitor> Visitors { get; set; }

    public RepositoryContext(DbContextOptions options) : base(options)
    {
         
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new VisitorConfiguration());
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new VisitDetailConfiguration());
    }
}
