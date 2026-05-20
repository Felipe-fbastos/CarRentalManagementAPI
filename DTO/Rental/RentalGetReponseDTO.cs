namespace CarRentalManagementAPI.DTO.Rental
{
    public class RentalGetReponseDTO
    {
        public int Id { get; set; }
        public string NameClient { get; set; }       
        public string NameCar { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
