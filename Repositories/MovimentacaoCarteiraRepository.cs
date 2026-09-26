using EmprestimoLibrary.Data;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLibrary.Repositories
{
    public class MovimentacaoCarteiraRepository : IMovimentacaoCarteiraRepository
    {
        private readonly ControleEmprestimoLivroContext _context;

        public MovimentacaoCarteiraRepository(ControleEmprestimoLivroContext context)
        {
            _context = context;
        }

        public async Task<(List<MovimentacaoCarteira> Dados, int Total)> BuscarPorCarteiraAsync(int idCarteira, int pagina, int tamanhoPagina)
        {
            var query = _context.Movimentacoes
                .Where(m => m.IdCarteira == idCarteira);

            var total = await query.CountAsync();

            var dados = await query
                .OrderByDescending(m => m.DataMovimento)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (dados, total);
        }

        public async Task<MovimentacaoCarteira> AdicionarAsync(MovimentacaoCarteira movimentacao)
        {
            await _context.Movimentacoes.AddAsync(movimentacao);
            await _context.SaveChangesAsync();
            return movimentacao;
        }
    }
}