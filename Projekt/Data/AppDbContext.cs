

using Microsoft.EntityFrameworkCore;
using Projekt.Entity;

namespace Projekt.Data;

public class AppDbContext : DbContext
{
    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    //DbSetsOfAllEntities
    DbSet<ClientPerson> ClientPersons { get; set; }
    DbSet<ClientCompany> ClientCompanies { get; set; }
    DbSet<Client> Clients { get; set; }
    DbSet<Contract> Contracts { get; set; }

    //Either apply configuration or use fluent API(modelBuilder.Entity<T>())
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }

    public override int SaveChanges()
    {
        HandleDeletes();
        HandleUpdates();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        HandleDeletes();
        HandleUpdates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void HandleDeletes()
    {
        //This is extra protection against deleting ClientCompany
        var entries = ChangeTracker.Entries<IBlockDelete>()
            .Where(e=> e.State == EntityState.Deleted);
        if (entries.Any()) throw new Exception("Cannot delete this entity");
        
        
        var softDelete = ChangeTracker.Entries<ISoftDelete>()
            .Where(e => e.State == EntityState.Deleted);
        foreach (var entry in softDelete)
        {
            //We use a method on every entity that implements ISoftDelete
            entry.Entity.SoftDelete();
        }
        
    }
    //Extra protection against updating KRS and Pesel
    private void HandleUpdates()
    {
        //Block from updating KRS
        var companies = ChangeTracker.Entries<ClientCompany>()
            .Where(e => e.State == EntityState.Modified);
        foreach (var entry in companies)
        {
            entry.Property(x => x.KRS).IsModified = false;
        }
        
        //Block from updating Pesel
        var persons = ChangeTracker.Entries<ClientPerson>()
            .Where(e => e.State == EntityState.Modified);
        foreach (var entry in persons)
        {
            entry.Property(x => x.Pesel).IsModified = false;
        }
        
        var contracts = ChangeTracker.Entries<Contract>()
            .Where(e => e.State == EntityState.Modified);
        if (contracts.Any()) throw new Exception("Cannot update Contract");
    }
}