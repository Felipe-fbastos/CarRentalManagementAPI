using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.Data;
using CarRentalManagementAPI.DTO.Client;
using CarRentalManagementAPI.Service;
using Mapster;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalManagementAPI.Controllers
{
    [Route("Client")]
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

        [HttpPost("SignUp")]
        public async Task<ActionResult<ClientSignUpDTO>> SignUp(ClientSignUpDTO dto)
        {
            // 1 - Impedir a persistencia da dados que já existem

            var existEmail = await _context.Clients.AnyAsync(p => p.Email == dto.Email);
            var existCPF = await _context.Clients.AnyAsync(p => p.CPF == dto.CPF);

            if (existEmail)
            {
                return Conflict("Email already registered");
            }

            if (existCPF)
            {
                return Conflict("CPF already registered");
            }

            // 2 - Converter de DTO para Entidade

            var user = dto.Adapt<Client>();

            // 3 - Criptografar a senha
            string hashString = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Password = hashString;

            // 4- Adicionar e salvar
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            var reponse = user.Adapt<ClientGetResponseDTO>();

            return Ok(reponse);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ClientLoginResponse>> Login(ClientLoginDTO dto)
        {
            // Buscar o meu usuário apartir de um email existente
            var user = await _context.Clients.FirstOrDefaultAsync(p => p.Email == dto.Email);

            // Validar se o email existe na minha base da dados
            if (user == null)
            {
                return Unauthorized("Invalid passaword or Email");
            }

            // Verifico se o hash da DTO bate com o do banco
            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            // Verifico se o a senha pertence ao email
            if (!validPassword)
            {
                return Unauthorized("Invalid passaword or Email");
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
