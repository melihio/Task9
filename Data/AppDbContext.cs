using Microsoft.EntityFrameworkCore;
using Task9.Models;

namespace Task9.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserNote> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).IsRequired().HasMaxLength(200);
            e.Property(u => u.Role).HasMaxLength(20);
            e.Property(u => u.CreatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<UserNote>(e =>
        {
            e.HasKey(n => n.Id);
            e.Property(n => n.Title).IsRequired().HasMaxLength(200);
            e.Property(n => n.CreatedAt).HasColumnType("datetime");
            e.HasOne(n => n.AppUser)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
