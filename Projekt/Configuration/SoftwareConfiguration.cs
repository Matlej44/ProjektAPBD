using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class SoftwareConfiguration : IEntityTypeConfiguration<Software>
{
    public void Configure(EntityTypeBuilder<Software> builder)
    {
        builder.ToTable("Software");
        builder.HasKey(x => x.SoftwareId);
        builder.HasOne(x => x.Client).WithMany(x => x.Software).HasForeignKey(x => x.ClientId);
        builder.HasOne(x => x.SoftwareType).WithMany(x => x.Software)
            .HasForeignKey(x => x.SoftwareTypeId);
        builder.Property(x => x.Name).HasMaxLength(50);
        
        builder.HasOne(x => x.CurrentVersion).WithMany().HasForeignKey(x => x.SoftwareVersionId)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}