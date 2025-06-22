using Microsoft.EntityFrameworkCore;
using backend.db.Models;

namespace backend.db;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add your DbSet properties here for your models
    public DbSet<HistoryTreasure> HistoryTreasures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure your models here
        modelBuilder.Entity<HistoryTreasure>().ToTable("history_treasures");
    }
} 