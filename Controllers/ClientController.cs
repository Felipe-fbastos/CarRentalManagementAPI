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
        public AppDbContext _context { get; set; }
        private readonly TokenService _tokenService;

        public ClientController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        
        [HttpGet("me")]
        public async Task<ActionResult<ClientGetResponseDTO>> Get()
        {
            var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userid == null)
            {
                return Unauthorized();
            }

            var user = await _context.Clients.FindAsync(int.Parse(userid));

            if(user == null)
            {
                return NotFound();
            }

            var response = user.Adapt<ClientGetResponseDTO>();

            return Ok(response);
            
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientGetResponseDTO>>> GetAll()
        {
            var client = await _context.Clients.ToListAsync();

            if (!client.Any())
            {
                return NotFound("Any client found");
            }

            var response = client.Adapt<List<ClientGetResponseDTO>>();

            return Ok(response);
        }

        [Authorize(Roles ="Administrator")]
        [HttpDelete("id:int")]
        public async Task<ActionResult<ClientGetResponseDTO>> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null || client.IsDelete)
            {
                return NotFound();
            }

            client.Delete();

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult<ClientSignUpDTO>> SignUp(ClientSignUpDTO dto)
        {
            // 1 - Impedir a persistencia da dados que já existem
            dto.Email = dto.Email.ToLower().Trim();

            bool existEmail = await _context.Clients.AnyAsync(p => p.Email == dto.Email);
            bool existCPF = await _context.Clients.AnyAsync(p => p.CPF == dto.CPF);

            if (existEmail)
            {
                return Conflict("Email already registered");
            }

            if (existCPF)
            {
                return Conflict("CPF already registered");
            }

            int age = DateTime.Today.Year - dto.BirthDate.Year;  //  Exemplo: DTO = 14/10/2008, DataAtual = 14/05/2026

            // DateTime.Today.AddYears(-age) - Pega a data de hoje e volta a quantidade de anos passada pela cálculo da variável Age
            
            // Se a data de aniversário for maior do que a data atual entra no if

            if (dto.BirthDate.Date > DateTime.Today.AddYears(-age)) // dto.BirthDate.Date - 14/05/2008 
            {
                // Se ainda não fez aniversário diminui 1 da idade
                age--;
            }

            if(age < 18)
            {
                return BadRequest("Clients under 18 are not allowed");
            }

            // 2 - Converter de DTO para Entidade

            var user = dto.Adapt<Client>();

            // 3 - Criptografar a senha
            string hashString = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Password = hashString;

            // 4- Adicionar e salvar
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var response = user.Adapt<ClientGetResponseDTO>();

            return Created("Client/me", response);
                
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ClientLoginResponse>> Login(ClientLoginDTO dto)
        {
            // Buscar o meu usuário a partir de um email existente
            var user = await _context.Clients.FirstOrDefaultAsync(p => p.Email == dto.Email);

            // Validar se o email existe na minha base da dados
            if (user == null)
            {
                return Unauthorized("Invalid password or Email");
            }

            // Verifico se o hash da DTO bate com o do banco
            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            // Verifico se o a senha pertence ao email
            if (!validPassword)
            {
                return Unauthorized("Invalid password or Email");
            }

            // Gero o token 
            var token = _tokenService.GenerateToken(user);


            // Retorno o token
            return Ok(new ClientLoginResponse
            {
               Token = token

            });

        }
        


    }
}


