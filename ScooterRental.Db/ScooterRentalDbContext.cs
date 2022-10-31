using Microsoft.EntityFrameworkCore;
using ScooterRental.Core.Models;

namespace ScooterRental.Db
{
    public class ScooterRentalDbContext : DbContext
    {
        public ScooterRentalDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Scooter> Scooters { get; set; }
        public DbSet<Rent> Rent { get; set; }
        public DbSet<Company> Company { get; set; }
    }
}