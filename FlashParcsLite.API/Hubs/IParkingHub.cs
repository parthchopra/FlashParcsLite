using System.Threading.Tasks;

namespace FlashParcsLite.API.Hubs
{
    public interface IParkingHub
    {
        Task GetLocationAsync(int locationId);
    }
}