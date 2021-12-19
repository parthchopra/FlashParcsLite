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

            if (context.Locations.Any())
            {
                return;
            }

            var locations = new Location[]
            {
                new Location { Name = "Menlo Park", Capacity = 10},
                new Location { Name = "Mountain View", Capacity = 10 },
                new Location { Name = "Cupertino", Capacity = 5 },
                new Location { Name = "Redmond", Capacity = 10 }
            };

            context.Locations.AddRange(locations);
            context.SaveChanges();
        }
    }
}
