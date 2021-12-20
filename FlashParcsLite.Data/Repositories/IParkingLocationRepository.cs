using System;
using System.Collections.Generic;
using FlashParcsLite.Data.Models;

namespace FlashParcsLite.Data.Repositories
{
    public interface IParkingLocationRepository
    {
        ParkingLocation GetParkingLocation(int parkingLocationId);
        ParkingLocation AddVehicle(int parkingLocationId);
        IEnumerable<ParkingLocation> GetParkingLocations();
        ParkingLocation RemoveVehicle(int parkingLocationId);
    }
}
