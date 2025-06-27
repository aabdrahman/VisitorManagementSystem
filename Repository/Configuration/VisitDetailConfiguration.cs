using Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Configuration;

public class VisitDetailConfiguration : IEntityTypeConfiguration<VisitDetail>
{
    public void Configure(EntityTypeBuilder<VisitDetail> builder)
    {
        builder.Property(v => v.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAdd();

        builder.Property(v => v.VisitationDate)
            .HasColumnType("date");
            
        builder.HasIndex(v => v.VisitorIdentificationNumber)
            .IsUnique();
        
        builder.Property(v => v.isDeleted)
            .HasDefaultValue(false);

        builder.Property(v => v.VisitorGender)
            .HasConversion<string>();
        builder.Property(v => v.VisitType)
            .HasConversion<string>();
        builder.Property(v => v.VisitorRegistrationType)
            .HasConversion<string>();
        builder.Property(v => v.VisitStatus)
            .HasConversion<string>();
    }
}
