using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EmprestimoLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("login")]
        [HttpPost("cliente/login")]
        public async Task<ActionResult<LoginResponseDto>> LoginCliente(LoginRequestDto loginDto)
        {
            try
            {
                var resultado = await _authService.LoginClienteAsync(loginDto);
                return Ok(resultado);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { erro = ex.Message });
            }
        }

        [AllowAnonymous]
        [EnableRateLimiting("login")]
        [HttpPost("editora/login")]
        public async Task<ActionResult<LoginResponseDto>> LoginEditora(LoginEditoraRequestDto loginDto)
        {
            try
            {
                var resultado = await _authService.LoginEditoraAsync(loginDto);
                return Ok(resultado);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { erro = ex.Message });
            }
        }
    }
}