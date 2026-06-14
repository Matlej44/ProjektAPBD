using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class SoftwareVersionConfiguration : IEntityTypeConfiguration<SoftwareVersion>
{
    public void Configure(EntityTypeBuilder<SoftwareVersion> builder)
    {
        builder.ToTable("SoftwareVersions");
        builder.HasKey(x => x.SoftwareVersionId);
        builder.Property(x => x.Version).HasMaxLength(30);
        builder.HasOne(x => x.Softwares).WithMany(x => x.SoftwareVersions)
            .HasForeignKey(x => x.SoftwareId);
        builder.HasIndex(x => new { x.SoftwareId, x.Version }).IsUnique();
        
        builder.HasData(
            // FinManager Pro versions
            new SoftwareVersion { SoftwareVersionId = 1, SoftwareId = 1, Version = "1.0.0", ReleaseDate = new DateTime(2022, 1, 1), YearlyPrice = 5000m },
            new SoftwareVersion { SoftwareVersionId = 2, SoftwareId = 1, Version = "2.0.0", ReleaseDate = new DateTime(2023, 6, 1), YearlyPrice = 6000m },
            new SoftwareVersion { SoftwareVersionId = 3, SoftwareId = 1, Version = "3.0.0", ReleaseDate = new DateTime(2024, 3, 1), YearlyPrice = 7000m },
            // EduLearn versions
            new SoftwareVersion { SoftwareVersionId = 4, SoftwareId = 2, Version = "1.0.0", ReleaseDate = new DateTime(2021, 9, 1), YearlyPrice = 3000m },
            new SoftwareVersion { SoftwareVersionId = 5, SoftwareId = 2, Version = "2.0.0", ReleaseDate = new DateTime(2023, 9, 1), YearlyPrice = 4000m }
        );
    }
}