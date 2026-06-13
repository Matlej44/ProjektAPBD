using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class SubscriptionPaymentConfiguration : IEntityTypeConfiguration<SubscriptionPayment>
{
    public void Configure(EntityTypeBuilder<SubscriptionPayment> builder)
    {
        builder.ToTable("SubscriptionPayments");
        builder.HasKey(x => x.SubscriptionPaymentId);
        builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        builder.HasOne(x => x.Subscription).WithMany(x => x.SubscriptionPayments)
            .HasForeignKey(x => x.SubscriptionId);
        
    }
}