using System;
using System.Text.Json.Serialization;

namespace FlashParcsLite.API.Models
{
    public class VehicleUpdateDto
    {
        public int ParkingLocationId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UpdateVehicleCount UpdateVehicleCount { get; set; }
    }

    public enum UpdateVehicleCount
    {
        Increase,
        Decrease
    }
}
