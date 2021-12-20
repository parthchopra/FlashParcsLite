using System.Threading.Tasks;

namespace FlashParcsLite.API.Hubs
{
    public interface IParkingHub
    {
        Task DecreaseVehicleCount(int locationId);
        Task GetLocationAsync(int locationId);
        Task IncreaseVehicleCount(int locationId);
    }
}