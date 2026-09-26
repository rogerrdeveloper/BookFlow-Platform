using EmprestimoLibrary.Models;

namespace EmprestimoLibrary.Repositories.interfaces
{
    public interface IEditoraRepository
    {
        Task<List<Editora>> BuscarTodosAsync();
        Task<Editora?> BuscarPorIdAsync(int id);
        Task<Editora?> BuscarPorEmailAsync(string email);
        Task<bool> ExistePorEmailAsync(string email, int? idEditora = null);
        Task<Editora> AdicionarAsync(Editora editora);
        Task AtualizarAsync(Editora editora);
    }
}
