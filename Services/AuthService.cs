using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Repositories.interfaces;
using Microsoft.IdentityModel.Tokens;

namespace EmprestimoLibrary.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEditoraRepository _editoraRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IClienteRepository clienteRepository,
            IEditoraRepository editoraRepository,
            IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _editoraRepository = editoraRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginClienteAsync(LoginRequestDto loginDto)
        {
            var cliente = await _clienteRepository.BuscarPorEmailAsync(loginDto.Email);

            if (cliente is null)
                throw new UnauthorizedAccessException("Email ou senha inválidos");

            if (!cliente.SituacaoConta)
                throw new UnauthorizedAccessException("Conta desativada");

            bool senhaValida = BCrypt.Net.BCrypt.Verify(loginDto.Senha, cliente.SenhaCliente);

            if (!senhaValida)
                throw new UnauthorizedAccessException("Email ou senha inválidos");

            var claims = new[]
            {
                new Claim("idCliente", cliente.IdCliente.ToString()),
                new Claim(ClaimTypes.Email, cliente.EmailCliente),
                new Claim(ClaimTypes.Role, "Cliente")
            };

            var token = GerarToken(claims);
            var expiracaoHoras = int.Parse(_configuration["Jwt:ExpiracaoHoras"]!);

            return new LoginResponseDto(
                token,
                cliente.NomeCliente,
                cliente.EmailCliente,
                DateTime.Now.AddHours(expiracaoHoras)
            );
        }

        public async Task<LoginResponseDto> LoginEditoraAsync(LoginEditoraRequestDto loginDto)
        {
            var editora = await _editoraRepository.BuscarPorEmailAsync(loginDto.Email);

            if (editora is null)
                throw new UnauthorizedAccessException("Email ou senha inválidos");

            bool senhaValida = BCrypt.Net.BCrypt.Verify(loginDto.Senha, editora.SenhaEditora);

            if (!senhaValida)
                throw new UnauthorizedAccessException("Email ou senha inválidos");

            var claims = new[]
            {
                new Claim("idEditora", editora.IdEditora.ToString()),
                new Claim(ClaimTypes.Email, editora.EmailEditora),
                new Claim(ClaimTypes.Role, "Editora")
            };

            var token = GerarToken(claims);
            var expiracaoHoras = int.Parse(_configuration["Jwt:ExpiracaoHoras"]!);

            return new LoginResponseDto(
                token,
                editora.NomeEditora,
                editora.EmailEditora,
                DateTime.Now.AddHours(expiracaoHoras)
            );
        }

        // Método privado genérico — evita duplicação, recebe as claims de cada login
        private string GerarToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiracaoHoras = int.Parse(_configuration["Jwt:ExpiracaoHoras"]!);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(expiracaoHoras),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}