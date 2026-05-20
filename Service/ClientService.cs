using CarLocationManagementAPI.Models;
using CarRentalManagementAPI.Data;
using CarRentalManagementAPI.DTO.Client;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarRentalManagementAPI.Service
{
    public class ClientService
    {
        public AppDbContext _context { get; set; }
        private readonly TokenService _tokenService;

        public ClientService(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<ClientGetResponseDTO> GetAsync(int userID)
        {
            var user = await _context.Clients.FirstOrDefaultAsync(p => p.Id == userID);
            
            if (userID == null)
            {
                throw new Exception("User not found");
            }

            return user.Adapt<ClientGetResponseDTO>();
        }

        public async Task<IEnumerable<ClientGetResponseDTO>> GetAllAsync()
        {
            var client = await _context.Clients.ToListAsync();

            if (!client.Any())
            {
                throw new Exception("Any client found");
            }

            return client.Adapt<List<ClientGetResponseDTO>>();
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null || client.IsDelete)
            {
                throw new Exception("Client delete or not found");
            }

            client.Delete();

            await _context.SaveChangesAsync();
        }

        public async Task<ClientGetResponseDTO> CreateAsync(ClientSignUpDTO dto)
        {
            // 1 - Impedir a persistencia da dados que já existem
            dto.Email = dto.Email.ToLower().Trim();

            bool existEmail = await _context.Clients.AnyAsync(p => p.Email == dto.Email);
            bool existCPF = await _context.Clients.AnyAsync(p => p.CPF == dto.CPF);

            if (existEmail)
            {
                throw new Exception("Email already registered");
            }

            if (existCPF)
            {
                throw new Exception("CPF already registered");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);   // Pega a data de hoje
            int age = DateTime.Today.Year - dto.BirthDate.Year;  //  Exemplo: DTO = 14/10/2008, DataAtual = 14/05/2026

            // DateTime.Today.AddYears(-age) - Pega a data de hoje e volta a quantidade de anos passada pela cálculo da variável Age

            // Se a data de aniversário for maior do que a data atual entra no if

            if (dto.BirthDate > today.AddYears(-age)) // dto.BirthDate.Date - 14/05/2008 
            {
                // Se ainda não fez aniversário diminui 1 da idade
                age--;
            }

            if (age < 18)
            {
                 throw new Exception("Clients under 18 are not allowed");
            }

            // 2 - Converter de DTO para Entidade

            var user = dto.Adapt<Client>();

            // 3 - Criptografar a senha
            string hashString = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Password = hashString;

            // 4- Adicionar e salvar
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.Adapt<ClientGetResponseDTO>();
        }

        public async Task<ClientLoginResponseDTO> LoginAsync(ClientLoginDTO dto)
        {
            // Buscar o meu usuário a partir de um email existente
            var user = await _context.Clients.FirstOrDefaultAsync(p => p.Email == dto.Email);

            // Validar se o email existe na minha base da dados
            if (user == null)
            {
                throw new Exception("Invalid password or Email");
            }

            // Verifico se o hash da DTO bate com o do banco
            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            // Verifico se o a senha pertence ao email
            if (!validPassword)
            {
                throw new Exception("Invalid password or Email");
            }

            // Gero o token 
            var token = _tokenService.GenerateToken(user);

            return new ClientLoginResponseDTO
            {
                Token = token,
            };

        }


    }
}
