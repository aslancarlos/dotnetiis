using Microsoft.EntityFrameworkCore;
using ProductApp.Models;

namespace ProductApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products"); // Nome da tabela no MariaDB

                entity.Property(p => p.Name)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(p => p.Price)
                      .HasColumnType("decimal(10,2)");

                entity.Property(p => p.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}

