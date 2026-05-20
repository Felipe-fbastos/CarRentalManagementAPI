using CarRentalManagementAPI.Models;
using System.Text.Json.Serialization;

namespace CarLocationManagementAPI.Models
{
    public class Car
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public int ManufacturingYear { get; set; }
        public string Color { get; set; }
        public string PlateNumber { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        public List<Rental> Rentals { get; set; } = new List<Rental>();
        public bool IsDeleted { get; private set; }

        public void Delete()
        {
            IsDeleted = true;
        }


    }
}

