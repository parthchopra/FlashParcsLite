using System;
using FlashParcsLite.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashParcsLite.Data
{
    public class ParkingContext : DbContext
    {
        //public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingLocation> ParkingLocations { get; set; }

        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Vehicle>().ToTable(nameof(Vehicle));
            modelBuilder.Entity<ParkingLocation>().ToTable(nameof(ParkingLocation));
        }
    }
}
