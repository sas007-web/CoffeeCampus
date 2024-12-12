using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CoffeeCampus
{
    public class CoffeeCampusContext : DbContext
    {
        public CoffeeCampusContext(DbContextOptions<CoffeeCampusContext> options)
            : base(options) {
        }

        // Tilføj DbSet for dine entiteter her
        public DbSet<User> User { get; set; }
        public DbSet<Refill> Refills { get; set; }
        public DbSet<Cleaning> Cleaning { get; set; }
        public DbSet<Machine> Machine { get; set; }

    }
}
