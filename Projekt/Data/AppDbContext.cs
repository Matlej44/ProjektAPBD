

using Microsoft.EntityFrameworkCore;
using Projekt.Configuration;
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
    public DbSet<ClientPerson> ClientPersons { get; set; }
    public DbSet<ClientCompany> ClientCompanies { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<SoftwareType> SoftwareTypes { get; set; }
    public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
    public DbSet<SubscriptionOffer> SubscriptionOffers { get; set; }
    public DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Employee> Employees { get; set; }
    

    //Either apply configuration or use fluent API(modelBuilder.Entity<T>())
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new ClientCompanyConfiguration());
        modelBuilder.ApplyConfiguration(new ClientPersonConfiguration());
        modelBuilder.ApplyConfiguration(new ContractConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new SoftwareConfiguration());
        modelBuilder.ApplyConfiguration(new SoftwareTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SoftwareVersionConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionOfferConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionPaymentConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        base.OnModelCreating(modelBuilder);
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
        if (entries.Any()) throw new InvalidOperationException("Cannot delete this entity");
        
        
        var softDelete = ChangeTracker.Entries<ISoftDelete>()
            .Where(e => e.State == EntityState.Deleted);
        foreach (var entry in softDelete)
        {
            //We use a method on every entity that implements ISoftDelete
            entry.Entity.SoftDelete();
            entry.State = EntityState.Modified;
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
            if (!entry.Entity.IsSoftDeleted)
                entry.Property(x => x.Pesel).IsModified = false;
        }
        
        var contracts = ChangeTracker.Entries<Contract>()
            .Where(e => e.State == EntityState.Modified);
        if (contracts.Any()) throw new InvalidOperationException("Cannot update Contract");
    }
}