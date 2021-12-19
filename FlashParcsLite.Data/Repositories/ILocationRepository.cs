using System;
using System.Collections.Generic;
using FlashParcsLite.Data.Models;

namespace FlashParcsLite.Data.Repositories
{
    public interface ILocationRepository
    {
        Location GetLocation(int locationId);
        Location AddVehicle(int locationId);
        List<string> GetLocationNames();
        List<Location> GetLocations();
        int GetVehicleCount(int locationId);
        Location RemoveVehicle(int locationId);
    }
}
