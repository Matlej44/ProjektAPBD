using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class ClientPersonConfiguration : IEntityTypeConfiguration<ClientPerson>
{
    public void Configure(EntityTypeBuilder<ClientPerson> builder)
    {
        builder.ToTable("ClientPerson");
        builder.HasKey(x => x.ClientId);
        builder.HasIndex(x => x.Pesel).IsUnique();
        builder.Property(x => x.Pesel).HasMaxLength(11).IsFixedLength();
    }
}