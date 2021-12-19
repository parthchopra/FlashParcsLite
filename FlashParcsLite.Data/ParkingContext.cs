using System;
using FlashParcsLite.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashParcsLite.Data
{
    public class ParkingContext : DbContext
    {
        //public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Location> Locations { get; set; }

        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Vehicle>().ToTable(nameof(Vehicle));
            modelBuilder.Entity<Location>().ToTable(nameof(Location));
        }
    }
}
