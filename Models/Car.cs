using CarRentalManagementAPI.Models;
using System.Text.Json.Serialization;

namespace CarLocationManagementAPI.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Model { get; set; }
        public int ManufacturingYear { get; set; }
        public string Color { get; set; }
        public string PlateNumber { get; set; }
        [JsonIgnore]
        public List<Rental> Rentals { get; set; } = new List<Rental>();

    }
}
