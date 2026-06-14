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
        builder.HasData(
            // Anna Nowak - płatność za styczeń (EduLearn miesięczny 300 PLN)
            new SubscriptionPayment
            {
                SubscriptionPaymentId = 1,
                SubscriptionId = 1,
                PaymentDate = new DateTime(2024, 1, 1),
                PeriodStartDate = new DateTime(2024, 1, 1),
                PeriodEndDate = new DateTime(2024, 2, 1),
                Amount = 300m
            },
            // EduSoft S.A. - płatność za rok (FinManager roczny 5000 PLN)
            new SubscriptionPayment
            {
                SubscriptionPaymentId = 2,
                SubscriptionId = 2,
                PaymentDate = new DateTime(2024, 1, 1),
                PeriodStartDate = new DateTime(2024, 1, 1),
                PeriodEndDate = new DateTime(2025, 1, 1),
                Amount = 5000m
            }
        );
    }
}