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
        builder.HasOne(x => x.SoftwareType).WithMany(x => x.Software)
            .HasForeignKey(x => x.SoftwareTypeId);
        builder.Property(x => x.Name).HasMaxLength(50);
        
        builder.HasData(
            new { SoftwareId = 1, Name = "FinManager Pro", Description = "System zarządzania finansami dla firm", SoftwareTypeId = 1 },
            new { SoftwareId = 2, Name = "EduLearn", Description = "Platforma e-learningowa dla szkół i uczelni", SoftwareTypeId = 2 }
        );
        
    }
}