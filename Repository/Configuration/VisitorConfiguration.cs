using Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        builder.Property(x => x.Gender)
            .HasConversion<string>();

        builder.Property(vi => vi.VisitorEmailAddress)
            .IsRequired(false);
    }
}
