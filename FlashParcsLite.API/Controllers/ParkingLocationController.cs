using System;
using System.Linq;
using System.Threading.Tasks;
using FlashParcsLite.API.Hubs;
using FlashParcsLite.API.Models;
using FlashParcsLite.Data.Models;
using FlashParcsLite.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace FlashParcsLite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingLocationController : ControllerBase
    {
        private readonly ILogger<ParkingLocationController> _logger;
        private readonly IParkingLocationRepository _parkingLocationRepository;
        private readonly IHubContext<ParkingHub> _parkingHub;

        public ParkingLocationController(
            ILogger<ParkingLocationController> logger,
            IParkingLocationRepository parkingLocationRepository,
            IHubContext<ParkingHub> hubContext)
        {
            _logger = logger;
            _parkingLocationRepository = parkingLocationRepository;
            _parkingHub = hubContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("request to return parking locations from API");
            var locations = _parkingLocationRepository.GetParkingLocations();

            if(locations == null || !locations.Any())
            {
                return NotFound();
            }

            return Ok(locations);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetParkingLocationById(int id)
        {
            _logger.LogInformation($"request to return parking location: {id} from API");

            try
            {
                var location = _parkingLocationRepository.GetParkingLocation(id);
                return Ok(location);
            }
            catch(Exception ex)
            {
                _logger.LogError(exception: ex, message: $"location not found for id: {id}");
                return NotFound();
            }            
        }

        [HttpPost("updateVehicleCount")]
        public async Task<IActionResult> UpdateVehicleCountAsync(VehicleUpdateDto vehicleUpdateDto)
        {
            _logger.LogInformation($"request to increase vehicle count on location {vehicleUpdateDto.ParkingLocationId}");

            try
            {
                ParkingLocation parkingLocation;
                switch (vehicleUpdateDto.UpdateVehicleCount)
                {
                    case UpdateVehicleCount.Increase:
                        parkingLocation = _parkingLocationRepository.AddVehicle(vehicleUpdateDto.ParkingLocationId);
                        break;
                    case UpdateVehicleCount.Decrease:
                        parkingLocation = _parkingLocationRepository.RemoveVehicle(vehicleUpdateDto.ParkingLocationId);
                        break;
                    default:
                        _logger.LogError(message: $"unknown action {vehicleUpdateDto.UpdateVehicleCount}");
                        return new BadRequestResult();          
                }

                await _parkingHub.Clients.All.SendAsync("ReceiveLocationInfo", parkingLocation, null);
                return Ok(parkingLocation);
            }
            catch(Exception ex)
            {
                _logger.LogError(exception: ex, message: "unable to update vehicle count");
                return new BadRequestObjectResult(error: ex.Message);
            }
        }
    }
}
