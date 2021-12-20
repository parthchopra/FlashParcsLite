using System;
using System.Collections.Generic;
using System.Linq;
using FlashParcsLite.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashParcsLite.Data.Repositories
{
    public class ParkingLocationRepository : IParkingLocationRepository
    {
        private readonly ParkingContext context;

        public ParkingLocationRepository(ParkingContext context)
        {
            this.context = context;
        }

        public IEnumerable<ParkingLocation> GetParkingLocations()
        {
            return context.ParkingLocations.ToList();
        }

        public ParkingLocation AddVehicle(int parkingLocationId)
        {
            var location = GetParkingLocation(parkingLocationId);
            location.VehicleCount += 1;

            if(location.VehicleCount > location.Capacity)
            {
                throw new InvalidOperationException($"Cannot add vehicle to the parking location: {location.Name} as it has reached its capacity");
            }

            UpdateLocation(location);
            return location;
        }

        public ParkingLocation RemoveVehicle(int parkingLocationId)
        {
            var location = GetParkingLocation(parkingLocationId);

            if (location.VehicleCount <= 0)
            {
                throw new InvalidOperationException($"Cannot remove vehicle from location: {location.Name} as the lot is empty");
            }

            location.VehicleCount -= 1;
            UpdateLocation(location);
            return location;
        }

        private void UpdateLocation(ParkingLocation location)
        {
            var updatedLocation = context.ParkingLocations.Attach(location);
            updatedLocation.State = EntityState.Modified;
            context.SaveChanges();
        }

        public ParkingLocation GetParkingLocation(int parkingLocationId) => context.ParkingLocations.Single(l => l.Id == parkingLocationId);
    }
}
