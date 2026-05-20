using CarLocationManagementAPI.Models;

namespace CarRentalManagementAPI.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int IdClient { get; set; }
        public Client Client { get; set; }
        public int IdCar { get; set; }
        public Car Car { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
