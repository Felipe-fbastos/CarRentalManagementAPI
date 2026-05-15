using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.Data;
using CarRentalManagementAPI.DTO.Car;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Security.Cryptography.Xml;

namespace CarRentalManagementAPI.Controllers
{
    [Route("car")]
    [Authorize(Roles = "Administrator")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public AppDbContext _context { get; set; }

        public CarController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet("{id:int}")] // Garante que o id passado pela rota seja um int
        public async Task<ActionResult<CarGetResponseDTO>> Get(int id)
        {
            
            var car = await _context.Cars.FindAsync(id);

            if (car == null) 
            { 
                return NotFound();
            }

            var response = car.Adapt<CarGetResponseDTO>();

            return Ok(response);

        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarGetResponseDTO>>> GetAll()
        {
            var cars = await _context.Cars.ToListAsync();

            if(!cars.Any())
            {
                return NotFound();
            }

            var response = cars.Adapt<List<CarGetResponseDTO>>();

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CarCreateDTO>> CreateCar(CarCreateDTO dto)
        {
            dto.PlateNumber = dto.PlateNumber.ToUpper().Trim();
            

            bool existPlate = await _context.Cars.AnyAsync(p => p.PlateNumber.ToLower() == dto.PlateNumber);

            if (existPlate)
            {
                return Conflict("Plate number already registered");
            }

            var car = dto.Adapt<Car>();

            await _context.Cars.AddAsync(car);

            await _context.SaveChangesAsync();

            var response = car.Adapt<CarGetResponseDTO>();

           
            return CreatedAtAction(
                nameof(Get), // Transforma o Get em string ou seja "Get" em tempo de compilação
                new {id = car.Id}, // Monta o parametros necessários que o método usa para funcionar
                response
                );
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<CarUpdateDTO>> Update(int id,CarUpdateDTO dto)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            dto.Adapt(car);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // SoftDelete ao invés de excluir eu mudo o status do carro
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if(car == null || car.IsDeleted)
            {
                return NotFound();
            }

            car.Delete();

            await _context.SaveChangesAsync();

            return NoContent();

        }
        
    }
}
