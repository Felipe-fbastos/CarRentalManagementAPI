using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.Data;
using CarRentalManagementAPI.DTO.Car;
using CarRentalManagementAPI.Service;
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
        private readonly CarService _service;

        public CarController(AppDbContext context, CarService service)
        {
            _service = service;
        }

        
        [HttpGet("{id:int}")] // Garante que o id passado pela rota seja um int
        public async Task<ActionResult<CarGetResponseDTO>> Get(int id)
        {
            
            var car = await _service.GetById(id);

            return Ok(car);

        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarGetResponseDTO>>> GetAll()
        {
            var car = await _service.GetAll();

            return Ok(car);
        }

        [HttpPost]
        public async Task<ActionResult<CarGetResponseDTO>> CreateCar(CarCreateDTO dto)
        {
            var car = await _service.CreateAsync(dto);

            var response = car.Adapt<CarGetResponseDTO>();

            return CreatedAtAction(
                nameof(Get),
                new { Id = car.Id },
                response
                );
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<CarUpdateDTO>> Update(int id,CarUpdateDTO dto)
        {
            var car = await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        // SoftDelete ao invés de excluir eu mudo o status do carro
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();

        }
        
    }
}
