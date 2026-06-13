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
    }
}