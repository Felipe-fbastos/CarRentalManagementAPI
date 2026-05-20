using System.Text.Json.Serialization;

namespace CarRentalManagementAPI.DTO.Rental
{
    public class RentalCreateDTO
    {
        public int IdClient { get; set; }
        public int IdCar { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        [JsonIgnore]
        public decimal? TotalPrice { get; set; }
    }
}
