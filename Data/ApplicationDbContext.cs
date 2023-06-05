using CarStory.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace CarStory.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarRepairShop> CarRepairShops { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<RepairParts> RepairParts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repair>()
                .HasOne(p => p.CarRepairShop)
                .WithMany(b => b.PendingRepairs)
                .HasForeignKey(p => p.CarRepairShopId);

            modelBuilder.Entity<Repair>()
                .HasOne(p => p.CarRepairShop)
                .WithMany(b => b.RepairsHistory)
                .HasForeignKey(p => p.CarRepairShopId);
        }
    }
}