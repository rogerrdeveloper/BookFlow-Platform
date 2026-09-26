using EmprestimoLibrary.DTOs;

namespace EmprestimoLibrary.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginClienteAsync(LoginRequestDto loginDto);
        Task<LoginResponseDto> LoginEditoraAsync(LoginEditoraRequestDto loginDto);
    }
}
