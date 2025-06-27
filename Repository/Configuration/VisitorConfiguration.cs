using Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration;

public class VisitorConfiguration : IEntityTypeConfiguration<Visitor>
{
    public void Configure(EntityTypeBuilder<Visitor> builder)
    {
        builder.Property(vi => vi.VisitorPhoneNumber)
            .IsRequired();

        builder.HasIndex(vi => vi.VisitorPhoneNumber)
            .IsUnique();

        builder.Property(vi => vi.Status)
            .HasDefaultValue("active")
            .ValueGeneratedOnAdd();
        builder.Property(x => x.CreatedDate)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAdd();

        builder.Property(vi => vi.VisitorEmailAddress)
            .IsRequired(false);
    }
}
