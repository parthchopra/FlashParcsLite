using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlashParcsLite.UI.Models;
using FlashParcsLite.UI.Services;

namespace FlashParcsLite.UI.Controllers
{
    public class ParkingLocationController : Controller
    {

        private readonly ILogger<ParkingLocationController> _logger;
        private readonly IParkingLocationService _parkingLocationService;

        public ParkingLocationController(ILogger<ParkingLocationController> logger, IParkingLocationService parkingLocationService)
        {
            _logger = logger;
            _parkingLocationService = parkingLocationService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            _logger.LogInformation("request to return parking locations in UI");

            var parkingLocations = await _parkingLocationService.GetAllParkingLocations();

            return View(parkingLocations);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
