using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");
        builder.HasKey(x => x.ContractId);
        builder.HasOne(p => p.Client).WithMany(p => p.Contracts)
            .HasForeignKey(p => p.ClientId);
        builder.HasOne(p => p.SoftwareVersion).WithMany(p => p.Contracts)
            .HasForeignKey(p => p.SoftwareVersionId);
        builder.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
        
        builder.HasData(
            new Contract
            {
                ContractId = 1,
                ClientId = 1,
                SoftwareVersionId = 3, // FinManager Pro 3.0.0
                StartDate = new DateTime(2024, 1, 1),
                EndDate = new DateTime(2024, 1, 20),
                CreatedAt = new DateTime(2024, 1, 1),
                AdditionalSupportYears = 1,
                TotalPrice = 8000m // 7000 + 1000 za dodatkowy rok
            },
            new Contract
            {
                ContractId = 2,
                ClientId = 3,
                SoftwareVersionId = 5, // EduLearn 2.0.0
                StartDate = new DateTime(2024, 2, 1),
                EndDate = new DateTime(2024, 2, 28),
                CreatedAt = new DateTime(2024, 2, 1),
                AdditionalSupportYears = 0,
                TotalPrice = 4000m
            }
        );
    }
}