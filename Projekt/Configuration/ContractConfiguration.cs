using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");
        builder.HasOne(p => p.Client).WithMany(p => p.Contracts)
            .HasForeignKey(p => p.ClientId);
        builder.HasOne(p => p.SoftwareVersion).WithMany(p => p.Contracts)
            .HasForeignKey(p => p.SoftwareVersionId);
    }
}