using CarLocationManagementAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRentalManagementAPI.Service
{
    public class TokenService
    {

        // IConfiguration permite acessar dados do appsettings.json,
        // user-secrets e variáveis de ambiente
        private readonly IConfiguration _configuration;

        // Injeção de dependência da configuração da aplicação
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método responsável por gerar o token JWT
        public string GenerateToken(Client user)
        {
            // Claims são informações do usuário que ficarão armazenadas no token
            var claims = new[]
            {
            // Id do usuário
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

            new Claim(ClaimTypes.Email, user.Email),

            new Claim(ClaimTypes.Role, user.TypeUser.ToString())           
        };

            // Cria uma chave de segurança usando a Key configurada no Jwt:Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            // Cria as credenciais de assinatura do token
            // HmacSha256 é o algoritmo utilizado para assinar o JWT
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            // Criação do token JWT
            var token = new JwtSecurityToken(

                // Quem gerou o token
                issuer: _configuration["Jwt:Issuer"],

                // Para quem o token foi criado
                audience: _configuration["Jwt:Audience"],

                // Informações armazenadas dentro do token
                claims: claims,

                // Tempo de expiração do token
                expires: DateTime.UtcNow.AddHours(2),

                // Assinatura digital do token
                signingCredentials: credentials
            );

            // Converte o objeto JWT em string
            // Essa string será enviada para o cliente
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

