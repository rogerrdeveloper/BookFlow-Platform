using EmprestimoLibrary.DTOs;

namespace EmprestimoLibrary.Services
{
    public interface ICompraService
    {
        Task<PaginaResponseDto<CompraResponseDto>> BuscarPorClienteIdAsync(int idCliente, PaginacaoRequestDto paginacao);
        Task<CompraResponseDto?> BuscarPorIdAsync(int idCompra);
        Task<CompraResponseDto> ComprarAsync(CompraRequestDto compraDto);
    }
}