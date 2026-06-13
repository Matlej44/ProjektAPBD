using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class ClientCompanyConfiguration : IEntityTypeConfiguration<ClientCompany>
{
    public void Configure(EntityTypeBuilder<ClientCompany> builder)
    {
        builder.ToTable("ClientCompanies");
        builder.HasKey(x => x.ClientId);
        builder.HasIndex(x => x.KRS).IsUnique();
        builder.Property(x => x.KRS).HasMaxLength(10).IsFixedLength();
    }
}