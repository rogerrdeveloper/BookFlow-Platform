using EmprestimoLibrary.Models;

namespace EmprestimoLibrary.Repositories.interfaces
{
    public interface ICompraRepository
    {
        Task<(List<Compra> Dados, int Total)> BuscarPorClienteIdAsync(int idCliente, int pagina, int tamanhoPagina);
        Task<Compra?> BuscarPorIdAsync(int id);
        Task<Compra> AdicionarAsync(Compra compra);
        Task<List<Compra>> BuscarPorEditoraIdAsync(int idEditora);
        Task<decimal> TotalVendasPorEditoraAsync(int idEditora);
        Task<int> TotalLivrosVendidosPorEditoraAsync(int idEditora);
    }
}