using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Infrastructure.Data.Configurations;

public class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
{
    public void Configure(EntityTypeBuilder<AuditTrail> builder)
    {
        builder.HasIndex(e => e.EntityName);
        builder.Property(e => e.EntityName)
            .HasMaxLength(100);
        builder.Property(e => e.PrimaryKey)
            .HasMaxLength(100);
        builder.Property(e => e.TrailType)
            .HasConversion(new EnumToStringConverter<TrailType>());
        builder.Property(e => e.ChangedColumns)
            .HasColumnType("jsonb");
        builder.Property(e => e.OldValues)
            .HasColumnType("jsonb");
        builder.Property(e => e.NewValues)
            .HasColumnType("jsonb");
    }
}
