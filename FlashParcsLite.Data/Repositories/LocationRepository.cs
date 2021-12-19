using System;
using System.Collections.Generic;
using System.Linq;
using FlashParcsLite.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashParcsLite.Data.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ParkingContext context;

        public LocationRepository(ParkingContext context)
        {
            this.context = context;
        }

        public List<string> GetLocationNames()
        {
            return context.Locations.Select(l => l.Name).ToList();
        }

        public List<Location> GetLocations()
        {
            return context.Locations.ToList();
        }

        public int GetVehicleCount(int locationId)
        {
            return GetLocation(locationId).VehicleCount;
        }

        public Location AddVehicle(int locationId)
        {
            var location = GetLocation(locationId);
            location.VehicleCount += 1;
            UpdateLocation(location);
            return location;
        }

        public Location RemoveVehicle(int locationId)
        {
            var location = GetLocation(locationId);

            if (location.VehicleCount <= 0)
            {
                throw new InvalidOperationException("No vehicles in the location");
            }

            location.VehicleCount -= 1;
            UpdateLocation(location);
            return location;
        }

        private void UpdateLocation(Location location)
        {
            var updatedLocation = context.Locations.Attach(location);
            updatedLocation.State = EntityState.Modified;
            context.SaveChanges();
        }

        public Location GetLocation(int locationId) => context.Locations.Single(l => l.Id == locationId);
    }
}
