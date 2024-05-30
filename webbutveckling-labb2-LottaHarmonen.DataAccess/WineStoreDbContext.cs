using Microsoft.EntityFrameworkCore;
using webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess;

public class WineStoreDbContext : DbContext 
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Wine> Wines { get; set; }
    public DbSet<WineType> WineTypes { get; set; }

    public DbSet<OrderWine> OrderWines { get; set; }

    public WineStoreDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderWine>()
            .HasKey(op => new { op.OrderId, op.WineId });
    }


}