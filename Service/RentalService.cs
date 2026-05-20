using Azure;
using CarRentalManagementAPI.Data;
using CarRentalManagementAPI.DTO.Rental;
using CarRentalManagementAPI.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementAPI.Service
{
    public class RentalService
    {
        public AppDbContext _context { get; set; }

        public RentalService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<RentalGetReponseDTO>> GetAllAsync()
        {
            var rental = await _context.Rentals
                .Include(r => r.Client)
                .Include(r => r.Car)
                .ToListAsync();

            if(!rental.Any())
            {
                throw new Exception("Rentals not not found");
            }

            return rental.Adapt<List<RentalGetReponseDTO>>();
        }


        public async Task<RentalGetReponseDTO> GetByIdAsync(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Client)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                throw new Exception("Rental not found");
            }

            return rental.Adapt<RentalGetReponseDTO>();
        }

        public async Task<RentalGetReponseDTO> CreateAsync(RentalCreateDTO dto)
        {
            //Verifico a existencia do carro
            var existCar = await _context.Cars.FindAsync(dto.IdCar);

            if (existCar == null || existCar.IsDeleted)
            {
                throw new Exception("Car not found or is inactive");
            }

            //Verifico a existencia do Cliente
            var existClient = await _context.Clients.FindAsync(dto.IdClient);

            if (existClient == null || existClient.IsDelete)
            {
                throw new Exception("Client not found or account is inactive");
            }

            //Verifico se a data de inicio é menor do que data de hoje
            if (dto.StartDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new Exception("Date cannot be before today");
            }
            // Verifico se a data do fim é menor do que a data e inicio
            if (dto.EndDate <= dto.StartDate)
            {
                throw new Exception("Start date cannot be after the end date.");
            }

            // Faço o periodo de quanto tempo o carro ficará alugado
            int timeRental = dto.EndDate.DayNumber - dto.StartDate.DayNumber;

            if (timeRental > 90)
            {
                throw new Exception("Cannot rent a car for more than 90 days");
            }

            // Calculo o preço da locação
            decimal TotalPrice = existCar.Price * timeRental;

            // Verifico se a existe locação existente na data informada pelo cliente
            bool IsCarUnavailable = await _context.Rentals
                .AnyAsync(r => r.IdCar == dto.IdCar &&
                                dto.StartDate <= r.EndDate &&
                                dto.EndDate >= r.StartDate);

            if (IsCarUnavailable)
            {
                throw new Exception("The car is already rented or reserved for the selected period.");
            }

            // Verifico  já se existia locação existente do cliente vigente
            bool IsClientUnavailable = await _context.Rentals
                .AnyAsync(r => r.IdClient == dto.IdClient &&
                               dto.StartDate <= r.EndDate &&
                               dto.EndDate >= r.StartDate);

            if (IsClientUnavailable)
            {
                throw new Exception("The client already has a rental during the selected period.");
            }

            var rental = dto.Adapt<Rental>();
            rental.TotalPrice = TotalPrice;

            await _context.Rentals.AddAsync(rental);

            await _context.SaveChangesAsync();

            rental = await _context.Rentals
                .AsNoTracking()
                .Include(r => r.Client)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == rental.Id);

            return rental.Adapt<RentalGetReponseDTO>();

            
        }
    }
}
