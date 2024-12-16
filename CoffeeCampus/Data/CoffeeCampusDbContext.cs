using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CoffeeCampus.Data
{
    public class CoffeeCampusDbContext : IdentityDbContext<Admin, IdentityRole, string>
    {
        public CoffeeCampusDbContext(DbContextOptions<CoffeeCampusDbContext> options)
            : base(options) {
        }

        public DbSet<Refill> Refills { get; set; }
        public DbSet<Cleaning> Cleaning { get; set; }
        public DbSet<CoffeeMachine> CoffeeMachines { get; set; }
        public DbSet<HoseChange> HoseChanges { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder); // Ensure Identity tables are configured

            builder.Entity<Refill>().ToTable("Refill");
            builder.Entity<Cleaning>().ToTable("Cleaning");
            builder.Entity<CoffeeMachine>().ToTable("CoffeeMachine");
            builder.Entity<HoseChange>().ToTable("HoseChange");
            builder.Entity<Service>().ToTable("Service");
        }
    }
}
