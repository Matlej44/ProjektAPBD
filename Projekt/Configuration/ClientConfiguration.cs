using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.HasKey(x => x.ClientId);
        builder.UseTptMappingStrategy();
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);
    }
}