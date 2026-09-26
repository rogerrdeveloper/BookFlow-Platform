using EmprestimoLibrary.Models;

namespace EmprestimoLibrary.Repositories.interfaces
{
    public interface IEmprestimoRepository
    {
        Task<(List<Emprestimo> Dados, int Total)> BuscarTodosAsync(int pagina, int tamanhoPagina);
        Task<Emprestimo?> BuscarPorIdAsync(int id);
        Task<Emprestimo> AdicionarAsync(Emprestimo emprestimo);
        Task AtualizarAsync(Emprestimo emprestimo);
        Task<(List<Emprestimo> Dados, int Total)> BuscarAtivosAsync(int pagina, int tamanhoPagina);
        Task<(List<Emprestimo> Dados, int Total)> BuscarInativosAsync(int pagina, int tamanhoPagina);
        Task<(List<Emprestimo> Dados, int Total)> BuscarAtrasadosAsync(DateTime dataLimite, int pagina, int tamanhoPagina);
        Task<List<Emprestimo>> BuscarAtrasadosSemMultaCalculadaAsync(int idCliente, DateTime dataLimite);
        Task<List<Emprestimo>> BuscarComMultaPendenteDeDescontoAsync(int idCliente);
        Task<bool> ExisteEmprestimoAtivoPorClienteAsync(int idCliente);
    }
}