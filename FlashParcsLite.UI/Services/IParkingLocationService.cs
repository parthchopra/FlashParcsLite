using System.Collections.Generic;
using System.Threading.Tasks;
using FlashParcsLite.UI.Models;

namespace FlashParcsLite.UI.Services
{
    public interface IParkingLocationService
    {
        Task<IEnumerable<ParkingLocation>> GetAllParkingLocations();
    }
}