namespace CarRentalManagementAPI.DTO.Car
{
    public class CarGetResponseDTO
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public int ManufacturingYear { get; set; }
        public string Color { get; set; }
        public string PlateNumber { get; set; }
        public decimal Price { get; set; }
    }
}
