using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class ClientCompanyConfiguration : IEntityTypeConfiguration<ClientCompany>
{
    public void Configure(EntityTypeBuilder<ClientCompany> builder)
    {
        builder.ToTable("ClientCompanies");
        builder.HasIndex(x => x.KRS).IsUnique();
        builder.Property(x => x.KRS).HasMaxLength(10).IsFixedLength();
        builder.HasData(
            new ClientCompany
            {
                ClientId = 3,
                CompanyName = "TechCorp Sp. z o.o.",
                Email = "kontakt@techcorp.pl",
                PhoneNumber = "22-300-400-500",
                KRS = "0000123456",
                Address = "ul. Przemysłowa 10, Warszawa"
            },
            new ClientCompany
            {
                ClientId = 4,
                CompanyName = "EduSoft S.A.",
                Email = "biuro@edusoft.pl",
                PhoneNumber = "12-400-500-600",
                KRS = "0000654321",
                Address = "ul. Akademicka 3, Kraków"
            }
        );
    }
}