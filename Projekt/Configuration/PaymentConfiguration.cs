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
        builder.HasOne(x => x.Contract).WithMany(x => x.Payments).HasForeignKey(x => x.ContractId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Client).WithMany(x => x.Payments).HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.NoAction);
        builder.HasData(

            new Payment { PaymentId = 1, ContractId = 1, ClientId = 1, Date = new DateTime(2024, 1, 5), Amount = 4000m },
            new Payment { PaymentId = 2, ContractId = 1, ClientId = 1, Date = new DateTime(2024, 1, 10), Amount = 4000m },

            new Payment { PaymentId = 3, ContractId = 2, ClientId = 3, Date = new DateTime(2024, 2, 5), Amount = 4000m }
        );
    }
}