using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class SoftwareTypeConfiguration : IEntityTypeConfiguration<SoftwareType>
{
    public void Configure(EntityTypeBuilder<SoftwareType> builder)
    {
        builder.ToTable("SoftwareTypes");
        builder.HasKey(x => x.SoftwareTypeId);
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.Name).HasMaxLength(50);

        builder.HasData(new List<SoftwareType>
        {
            new(){SoftwareTypeId = 1, Name = "Finanse"},
            new(){SoftwareTypeId = 2, Name = "Edukacja"}
        });
    }
}