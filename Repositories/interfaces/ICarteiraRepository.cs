using EmprestimoLibrary.Models;

namespace EmprestimoLibrary.Repositories.interfaces
{
    public interface ICarteiraRepository
    {
        Task<Carteira?> BuscarPorIdAsync(int id);
        Task<Carteira?> BuscarPorClienteIdAsync(int idCliente);
        Task<Carteira> AdicionarAsync(Carteira carteira);
        Task AtualizarAsync(Carteira carteira);
        Task<Carteira?> BuscarPorEditoraIdAsync(int idEditora);
    }
}
