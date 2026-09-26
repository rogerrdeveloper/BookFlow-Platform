using EmprestimoLibrary.DTOs;

namespace EmprestimoLibrary.Services
{
    public interface ICarteiraService
    {
        Task<CarteiraResponseDto?> BuscarPorClienteIdAsync(int idCliente);
        Task<CarteiraResponseDto> DepositarAsync(int idCliente, CarteiraOperacaoRequestDto operacao);
        Task<CarteiraResponseDto> SacarAsync(int idCliente, CarteiraOperacaoRequestDto operacao);
        Task<PaginaResponseDto<MovimentacaoCarteiraResponseDto>> ObterExtratoAsync(int idCliente, PaginacaoRequestDto paginacao);
        Task CobrarMultasPendentesAsync(int idCliente);

        // Métodos para Editora
        Task<CarteiraResponseDto?> BuscarPorEditoraIdAsync(int idEditora);
        Task<CarteiraResponseDto> SacarEditoraAsync(int idEditora, CarteiraOperacaoRequestDto operacao);
        Task<PaginaResponseDto<MovimentacaoCarteiraResponseDto>> ObterExtratoEditoraAsync(int idEditora, PaginacaoRequestDto paginacao);
    }
}