using EmprestimoLibrary.Models;

namespace EmprestimoLibrary.Repositories.interfaces
{
    public interface ILivroRepository
    {
        Task<(List<Livro> Dados, int Total)> BuscarTodosAsync(int pagina, int tamanhoPagina, string? titulo = null);
        Task<(List<Livro> Dados, int Total)> BuscarPorEditoraAsync(int idEditora, int pagina, int tamanhoPagina);
        Task<Livro?> BuscarPorIdAsync(int id);
        Task<Livro> AdicionarAsync(Livro livro);
        Task AtualizarAsync(Livro livro);
        Task ExcluirAsync(Livro livro);
    }
}
