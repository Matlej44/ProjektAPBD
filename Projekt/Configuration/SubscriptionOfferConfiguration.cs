using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class SubscriptionOfferConfiguration : IEntityTypeConfiguration<SubscriptionOffer>
{
    public void Configure(EntityTypeBuilder<SubscriptionOffer> builder)
    {
        builder.ToTable("SubscriptionOffers");
        builder.HasKey(x => x.SubscriptionOfferId);
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.HasOne(x => x.Software).WithMany(x => x.SubscriptionOffers).HasForeignKey(x => x.SoftwareId);
        builder.HasData(
            new SubscriptionOffer { SubscriptionOfferId = 1, Name = "FinManager Monthly", RenewalPeriod = 1, Price = 500m, SoftwareId = 1 },
            new SubscriptionOffer { SubscriptionOfferId = 2, Name = "FinManager Yearly", RenewalPeriod = 12, Price = 5000m, SoftwareId = 1 },
            new SubscriptionOffer { SubscriptionOfferId = 3, Name = "EduLearn Monthly", RenewalPeriod = 1, Price = 300m, SoftwareId = 2 },
            new SubscriptionOffer { SubscriptionOfferId = 4, Name = "EduLearn Yearly", RenewalPeriod = 12, Price = 3000m, SoftwareId = 2 }
        );
    }
}