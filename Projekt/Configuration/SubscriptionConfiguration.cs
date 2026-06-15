using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscriptions");
        builder.HasKey(x => x.SubscriptionId);
        builder.HasOne(x => x.SubscriptionOffer).WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.SubscriptionOfferId);
        builder.HasOne(x => x.Client).WithMany(x => x.Subscriptions)
            .HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.NoAction);
        
        builder.HasData(
            // Anna Nowak - subskrypcja miesięczna EduLearn
            new Subscription { SubscriptionId = 1, ClientId = 2, SubscriptionOfferId = 3, StartDate = new DateTime(2024, 1, 1), EndDate = new DateTime(2024, 2, 1)},
            // EduSoft S.A. - subskrypcja roczna FinManager
            new Subscription { SubscriptionId = 2, ClientId = 4, SubscriptionOfferId = 2, StartDate = new DateTime(2024, 1, 1), EndDate = new DateTime(2025, 1, 1)}
        );
        
    }
}