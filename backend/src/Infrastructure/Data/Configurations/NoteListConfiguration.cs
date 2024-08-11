using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations;

public class NoteListConfiguration : IEntityTypeConfiguration<NoteList>
{
    public void Configure(EntityTypeBuilder<NoteList> builder)
    {
        //builder.Property(t => t.Id)
        //    .ValueGeneratedNever();

        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
    }
}
