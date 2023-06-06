using CarStory.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace CarStory.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarRepairShop> CarRepairShops { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<RepairParts> RepairParts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repair>()
                .HasOne(p => p.CarRepairShop)
                .WithMany(b => b.AllRepairs)
                .HasForeignKey(p => p.CarRepairShopId);

            modelBuilder.Entity<RepairParts>()
                .HasKey(r => new { r.PartId, r.RepairId });

            base.OnModelCreating(modelBuilder);
        }
    }
}