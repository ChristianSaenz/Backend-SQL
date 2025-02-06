using Backend_SQL.Models;
using Microsoft.EntityFrameworkCore;



namespace Backend_SQL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Toy> Toys { get; set; }
        public DbSet<Cloth> Cloths { get; set; }   
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                 .HasOne(o => o.Toy)
                 .WithMany(t => t.Orders)
                 .HasForeignKey(o => o.ToyID)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cloth)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.ClothID)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
