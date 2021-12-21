using System;
using System.Linq;
using FlashParcsLite.Data.Models;

namespace FlashParcsLite.Data
{
    public static class DbSeeder
    {
        public static void Seed(ParkingContext context)
        {
            context.Database.EnsureCreated();

            if (context.ParkingLocations.Any())
            {
                return;
            }

            var locations = new ParkingLocation[]
            {
                new ParkingLocation { Name = "Menlo Park", Capacity = 10},
                new ParkingLocation { Name = "Mountain View", Capacity = 10 },
                new ParkingLocation { Name = "Cupertino", Capacity = 5 },
                new ParkingLocation { Name = "Redmond", Capacity = 10 }
            };

            context.ParkingLocations.AddRange(locations);
            context.SaveChanges();
        }
    }
}
