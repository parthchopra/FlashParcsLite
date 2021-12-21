using System;
using System.Threading.Tasks;
using FlashParcsLite.Data.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace FlashParcsLite.API.Hubs
{
    public class ParkingHub : Hub, IParkingHub
    {
        private readonly IParkingLocationRepository repo;

        public ParkingHub(IParkingLocationRepository repo)
        {
            this.repo = repo;
        }

        public async Task GetLocationAsync(int locationId)
        {
            var location = repo.GetParkingLocation(locationId);
            await Clients.All.SendAsync("ReceiveLocationInfo", location);
        }       
    }
}
