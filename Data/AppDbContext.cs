using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace CarRentalManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Sempre começar pelo lado N
            modelBuilder.Entity<Rental>()
                .HasOne(c => c.Client)
                .WithMany(r => r.Rentals)
                .HasForeignKey(c => c.IdClient)
                .IsRequired();

            modelBuilder.Entity<Rental>()
                .HasOne(c => c.Car)
                .WithMany(r => r.Rentals)
                .HasForeignKey(c => c.IdCar)
                .IsRequired();

            
            // Index Unique
            modelBuilder.Entity<Car>()
                .HasIndex(p => p.PlateNumber)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(p => p.CPF)
                .IsUnique();

            // Properts
            modelBuilder.Entity<Client>()
                .Property(p => p.CPF)
                .HasColumnType("char(11)")
                .IsRequired();

            modelBuilder.Entity<Car>()
                .Property(p => p.PlateNumber)
                .HasColumnType("char(7)")
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
