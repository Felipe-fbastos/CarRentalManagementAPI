using CarRentalManagementAPI.Data;
using CarRentalManagementAPI.DTO.Rental;
using CarRentalManagementAPI.Models;
using CarRentalManagementAPI.Service;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementAPI.Controllers
{
    [Route("Rental")]
    [Authorize]
    [ApiController]
    public class RentalController : ControllerBase
    {
        public AppDbContext _context { get; set; }
        private readonly RentalService _service;

        public RentalController(AppDbContext context, RentalService service)
        {
            _context = context;
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalGetReponseDTO>>> Get()
        {

            var response = await _service.GetAllAsync();

            return Ok(response);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RentalGetReponseDTO>> GetSingle(int id)
        {
            var response = await _service.GetByIdAsync(id);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<RentalGetReponseDTO>> CreateRental(RentalCreateDTO dto)
        {
            var response = await _service.CreateAsync(dto);
           
            return CreatedAtAction(
                nameof(GetSingle),
                new {id = response.Id},
                response                
                );
        }
    }
}
