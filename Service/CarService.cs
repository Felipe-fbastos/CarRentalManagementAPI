using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.Data;
using CarRentalManagementAPI.DTO.Car;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementAPI.Service
{
    public class CarService
    {
        public AppDbContext _context { get; set; }

        public CarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarGetResponseDTO> GetById(int id) 
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                throw new Exception("Car NotFound");
            }

            return car.Adapt<CarGetResponseDTO>(); 
        
        }

        public async Task<IEnumerable<CarGetResponseDTO>> GetAll()
        {
            var car = await _context.Cars.ToListAsync();

            if (!car.Any())
            {
                throw new Exception("Car NotFound"); 
            }

            var response = car.Adapt<List<CarGetResponseDTO>>();

            return response;
        }

        public async Task<CarGetResponseDTO> CreateAsync(CarCreateDTO dto)
        {
            dto.PlateNumber = dto.PlateNumber.ToUpper().Trim();
            dto.Color = dto.Color.ToUpper().Trim();
            int actualYear = DateOnly.FromDateTime(DateTime.Today).Year;

            bool existPlate = await _context.Cars.AnyAsync(p => p.PlateNumber.ToUpper() == dto.PlateNumber);

            if (existPlate)
            {
                throw new Exception("Plate number already registered");
            }
            else if (dto.Price <= 80)
            {
                throw new Exception("Minimum car price is 80");
            }
            else if (dto.ManufacturingYear > actualYear)
            {
                throw new Exception($"The car's year ({dto.ManufacturingYear}) cannot be greater than the current year ({actualYear})");
            }

            var car = dto.Adapt<Car>();

            await _context.Cars.AddAsync(car);

            await _context.SaveChangesAsync();

            return car.Adapt<CarGetResponseDTO>();
                
        }

        public async Task<CarGetResponseDTO> UpdateAsync(int id, CarUpdateDTO dto)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
               throw new Exception("Car not found");
            }

            dto.Adapt(car);

            await _context.SaveChangesAsync();
            
            return dto.Adapt<CarGetResponseDTO>();
        }

        public async Task DeleteAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if(car == null || car.IsDeleted)
            {
                throw new Exception($"Id cannot be null");
            }

            car.Delete();

            await _context.SaveChangesAsync();
            
        }
    }
}
