using EmprestimoLibrary.DTOs;

namespace EmprestimoLibrary.Services
{
    public interface IClienteService
    {
        Task<PaginaResponseDto<ClienteResponseDto>> BuscarTodosAsync(PaginacaoRequestDto pagina);
        Task<ClienteResponseDto?> BuscarPorIdAsync(int id);
        Task<ClienteResponseDto> AdicionarAsync(ClienteRequestDto clienteDto);
        Task AtualizarAsync(int id, ClienteAtualizarRequestDto cliente);
        Task DesativarContaAsync(int id);
    }
}