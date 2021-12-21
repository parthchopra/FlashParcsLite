using System;
namespace FlashParcsLite.UI.Models
{
    public class ParkingLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VehicleCount { get; set; }
        public int Capacity { get; set; }
    }
}
