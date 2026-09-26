using EmprestimoLibrary.DTOs;

namespace EmprestimoLibrary.Services
{
    public interface IEmprestimoService
    {
        Task<PaginaResponseDto<EmprestimoResponseDto>> BuscarTodosAsync(PaginacaoRequestDto paginacao);
        Task<EmprestimoResponseDto?> BuscarPorIdAsync(int id);
        Task<EmprestimoResponseDto> AdicionarAsync(EmprestimoRequestDto emprestimoDto);
        Task DevolverAsync(int id);
        Task<PaginaResponseDto<EmprestimoResponseDto>> BuscarAtivosAsync(PaginacaoRequestDto paginacao);
        Task<PaginaResponseDto<EmprestimoResponseDto>> BuscarInativosAsync(PaginacaoRequestDto paginacao);
        Task<PaginaResponseDto<EmprestimoResponseDto>> BuscarAtrasadosAsync(PaginacaoRequestDto paginacao);
    }
}