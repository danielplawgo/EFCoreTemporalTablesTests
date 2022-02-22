using Microsoft.EntityFrameworkCore;

namespace EFCoreTemporalTablesTests;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=EFCoreTemporalTablesTests;User=sa;Password=Your_password123;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .ToTable("Products", b => b.IsTemporal());
    }
}