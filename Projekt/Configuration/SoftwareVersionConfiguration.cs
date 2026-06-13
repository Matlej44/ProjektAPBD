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
        builder.HasOne(x => x.Software).WithMany(x => x.SoftwareVersions)
            .HasForeignKey(x => x.SoftwareId).OnDelete(DeleteBehavior.Cascade);
    }
}