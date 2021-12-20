using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlashParcsLite.UI.Models;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FlashParcsLite.UI.Controllers
{
    public class ParkingLocationController : Controller
    {

        private readonly ILogger<ParkingLocationController> _logger;
        private readonly HttpClient _httpClient;
        public ParkingLocationController(ILogger<ParkingLocationController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("parkingLocation");
        }

        public async Task<IActionResult> IndexAsync()
        {
            _logger.LogInformation("request to return parking locations in UI");
            var response = await _httpClient.GetAsync("/api/ParkingLocation");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var parkingLocations = JsonSerializer.Deserialize<IEnumerable<ParkingLocation>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
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
