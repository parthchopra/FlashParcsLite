using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashParcsLite.Data.Models
{
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int VehicleCount { get; set; }
        public int Capacity { get; set; }
    }
}
