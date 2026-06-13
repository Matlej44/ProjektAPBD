using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Entity;

namespace Projekt.Configuration;

public class SofrwareSellConfiguration : IEntityTypeConfiguration<SoftwareSell>
{
    public void Configure(EntityTypeBuilder<SoftwareSell> builder)
    {
        builder.ToTable("SoftwareSells");
        builder.HasKey(x => x.SoftwareSellId);
        
        
    }
}