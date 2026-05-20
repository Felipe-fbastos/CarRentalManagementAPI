using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.Data;
using CarRentalManagementAPI.DTO.Client;
using CarRentalManagementAPI.Service;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarRentalManagementAPI.Controllers
{
    
    [Route("client")]

    [Authorize]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }

        
        [HttpGet("me")]
        public async Task<ActionResult<ClientGetResponseDTO>> Get()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userid == null)
            {
                return Unauthorized();
            }

            var response = await _clientService.GetAsync(int.Parse(userid));

            return Ok(response);
            
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientGetResponseDTO>>> GetAll()
        {
            var response = await _clientService.GetAllAsync();

            return Ok(response);
        }

        [Authorize(Roles ="Administrator")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
           await _clientService.DeleteAsync(id);

            return NoContent();
        }


        [AllowAnonymous]
        [HttpPost("signup")]git
        public async Task<ActionResult<ClientSignUpDTO>> SignUp(ClientSignUpDTO dto)
        {
            
           var response = await _clientService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(Get),
                new {Id = response.Id},
                response        
                );
                
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ClientLoginResponseDTO>> Login(ClientLoginDTO dto)
        {

            var response = await _clientService.LoginAsync(dto);

            return Ok(response);

        }
        


    }
}


