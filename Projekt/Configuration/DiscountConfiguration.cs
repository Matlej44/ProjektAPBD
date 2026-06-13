using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.ToTable("Discounts");
        builder.HasKey(x => x.DiscountId);
        builder.HasMany(x => x.Software).WithMany(x => x.Discounts)
            .UsingEntity(entityEntryBuilder => entityEntryBuilder.ToTable("SoftwareDiscounts"));
    }
}