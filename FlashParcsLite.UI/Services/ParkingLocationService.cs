using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FlashParcsLite.UI.Models;

namespace FlashParcsLite.UI.Services
{
    public class ParkingLocationService : IParkingLocationService
    {
        private readonly HttpClient _httpClient;
        public ParkingLocationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("parkingLocation");
        }

        public async Task<IEnumerable<ParkingLocation>> GetAllParkingLocations()
        {
            var response = await _httpClient.GetAsync("/api/ParkingLocation");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var parkingLocations = JsonSerializer.Deserialize<IEnumerable<ParkingLocation>>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });

            return parkingLocations;
        }
    }
}
