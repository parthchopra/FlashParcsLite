using System;
using FlashParcsLite.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashParcsLite.Data
{
    public class ParkingContext : DbContext
    {
        public DbSet<ParkingLocation> ParkingLocations { get; set; }

        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingLocation>().ToTable(nameof(ParkingLocation));
        }
    }
}
