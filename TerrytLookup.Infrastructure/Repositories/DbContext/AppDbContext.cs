using Microsoft.EntityFrameworkCore;
using TerrytLookup.Core.Domain;

namespace TerrytLookup.Infrastructure.Repositories.DbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public const int PageSize = 20;

    public DbSet<Voivodeship> Voivodeships { get; init; }

    public DbSet<County> Counties { get; init; }

    public DbSet<Street> Streets { get; init; }

    public DbSet<Town> Towns { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("unaccent");

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var entries = ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries) entry.Entity.UpdateTimestamp = DateTimeOffset.UtcNow;

        return base.SaveChangesAsync(cancellationToken);
    }
}