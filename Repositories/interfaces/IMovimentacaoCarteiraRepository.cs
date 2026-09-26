using EmprestimoLibrary.Models;

namespace EmprestimoLibrary.Repositories.interfaces
{
    public interface IMovimentacaoCarteiraRepository
    {
        Task<(List<MovimentacaoCarteira> Dados, int Total)> BuscarPorCarteiraAsync(int idCarteira, int pagina, int tamanhoPagina);
        Task<MovimentacaoCarteira> AdicionarAsync(MovimentacaoCarteira movimentacao);
    }
}