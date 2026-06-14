using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class ClientPersonConfiguration : IEntityTypeConfiguration<ClientPerson>
{
    public void Configure(EntityTypeBuilder<ClientPerson> builder)
    {
        builder.ToTable("ClientPerson");
        builder.HasIndex(x => x.Pesel).IsUnique();
        builder.Property(x => x.Pesel).HasMaxLength(11).IsFixedLength();
        builder.HasData(
            new ClientPerson
            {
                ClientId = 1,
                Name = "Jan",
                Surname = "Kowalski",
                Email = "jan.kowalski@email.com",
                PhoneNumber = "500-100-200",
                Pesel = "90010112345"
            },
            new ClientPerson
            {
                ClientId = 2,
                Name = "Anna",
                Surname = "Nowak",
                Email = "anna.nowak@email.com",
                PhoneNumber = "600-200-300",
                Pesel = "85052267890"
            }
        );
    }
}