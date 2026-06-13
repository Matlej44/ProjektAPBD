using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(x => x.PaymentId);
        builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        builder.HasOne(x => x.Contract).WithMany(x => x.Payments).HasForeignKey(x => x.ContractId);
        builder.HasOne(x => x.Client).WithMany(x => x.Payments).HasForeignKey(x => x.ClientId);
        
    }
}