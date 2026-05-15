namespace CarRentalManagementAPI.DTO.Car
{
    public class CarUpdateDTO
    {
        public string? Name { get; set; }
        public string? Model { get; set; }
        public string? ManufacturingYear { get; set; }
        public string? Color { get; set; }
        public decimal? Price { get; set; }
    }
}
