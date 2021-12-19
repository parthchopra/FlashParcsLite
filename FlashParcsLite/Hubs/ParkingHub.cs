using System;
using System.Threading.Tasks;
using FlashParcsLite.Data.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace FlashParcsLite.Hubs
{
    public class ParkingHub : Hub
    {
        private readonly ILocationRepository repo;

        public ParkingHub(ILocationRepository repo)
        {
            this.repo = repo;
        }

        public async Task GetLocationAsync(int locationId)
        {
            var location = repo.GetLocation(locationId);
            await Clients.All.SendAsync("ReceiveLocationInfo", location);
        }

        public async Task IncreaseVehicleCount(int locationId)
        {
            var location = repo.AddVehicle(locationId);
            await Clients.All.SendAsync("ReceiveLocationInfo", location);
        }

        public async Task DecreaseVehicleCount(int locationId)
        {
            var location = repo.RemoveVehicle(locationId);
            await Clients.All.SendAsync("ReceiveLocationInfo", location);
        }
    }
}
