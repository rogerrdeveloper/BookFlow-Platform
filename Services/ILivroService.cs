using EmprestimoLibrary.DTOs;

namespace EmprestimoLibrary.Services
{
    public interface ILivroService
    {
        Task<PaginaResponseDto<LivroResponseDto>> BuscarTodosAsync(PaginacaoRequestDto paginacao);
        Task<PaginaResponseDto<LivroResponseDto>> BuscarPorEditoraAsync(int idEditora, PaginacaoRequestDto paginacao);
        Task<LivroResponseDto?> BuscarPorIdAsync(int id);
        Task<LivroResponseDto> AdicionarAsync(LivroRequestDto livroRequest, int idEditora);
        Task AtualizarAsync(int id, LivroRequestDto livroDto, int idEditora);
        Task ExcluirAsync(int id, int idEditora); 
    }
}
