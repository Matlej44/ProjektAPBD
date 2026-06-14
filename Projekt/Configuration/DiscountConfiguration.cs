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
        builder.HasMany(x => x.Software)
            .WithMany(x => x.Discounts)
            .UsingEntity<Dictionary<string, object>>(
                "SoftwareDiscounts",
                j => j.HasOne<Software>().WithMany().HasForeignKey("SoftwareId"),
                j => j.HasOne<Discount>().WithMany().HasForeignKey("DiscountId"),
                j =>
                {
                    j.ToTable("SoftwareDiscounts");
                    // Seed join table: FinManager Pro gets Black Friday, EduLearn gets Summer Sale
                    j.HasData(
                        new { SoftwareId = 1, DiscountId = 1 },
                        new { SoftwareId = 2, DiscountId = 2 }
                    );
                });
        builder.HasData(
            new Discount
            {
                DiscountId = 1,
                Name = "Black Friday Discount",
                DiscountPercent = 10m,
                StartDate = new DateTime(2024, 11, 25),
                EndDate = new DateTime(2024, 11, 30),
                IsRepetitive = true
            },
            new Discount
            {
                DiscountId = 2,
                Name = "Summer Sale",
                DiscountPercent = 15m,
                StartDate = new DateTime(2024, 7, 1),
                EndDate = new DateTime(2024, 7, 31),
                IsRepetitive = true
            });
    }
}