using EmprestimoLibrary.Data;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLibrary.Repositories
{
    public class CompraRepository : ICompraRepository
    {
        private readonly ControleEmprestimoLivroContext _context;

        public CompraRepository(ControleEmprestimoLivroContext context)
        {
            _context = context;
        }

        public async Task<(List<Compra> Dados, int Total)> BuscarPorClienteIdAsync(int idCliente, int pagina, int tamanhoPagina)
        {
            var query = _context.Compras
                .Where(c => c.IdCliente == idCliente);

            var total = await query.CountAsync();

            var dados = await query
                .OrderByDescending(c => c.DataCompra)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (dados, total);
        }

        public async Task<Compra?> BuscarPorIdAsync(int id)
        {
            return await _context.Compras.FindAsync(id);
        }

        public async Task<Compra> AdicionarAsync(Compra compra)
        {
            await _context.Compras.AddAsync(compra);
            await _context.SaveChangesAsync();
            return compra;
        }

        public async Task<List<Compra>> BuscarPorEditoraIdAsync(int idEditora)
        {
            return await _context.Compras
                .Where(c => _context.Livros
                    .Any(l => l.IdLivro == c.IdLivro && l.IdEditora == idEditora))
                .OrderByDescending(c => c.DataCompra)
                .ToListAsync();
        }

        public async Task<decimal> TotalVendasPorEditoraAsync(int idEditora)
        {
            return await _context.Compras
                .Where(c => _context.Livros
                    .Any(l => l.IdLivro == c.IdLivro && l.IdEditora == idEditora))
                .SumAsync(c => c.CompraValorTotal);
        }

        public async Task<int> TotalLivrosVendidosPorEditoraAsync(int idEditora)
        {
            return await _context.Compras
                .Where(c => _context.Livros
                    .Any(l => l.IdLivro == c.IdLivro && l.IdEditora == idEditora))
                .SumAsync(c => c.CompraQuantidade);
        }
    }
}