using EmprestimoLibrary.Data;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLibrary.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly ControleEmprestimoLivroContext _context;

        public EmprestimoRepository(ControleEmprestimoLivroContext context)
        {
            _context = context;
        }

        public async Task<(List<Emprestimo> Dados, int Total)> BuscarTodosAsync(int pagina, int tamanhoPagina)
        {
            var query = _context.Emprestimos.AsQueryable();
            var total = await query.CountAsync();
            var dados = await query
                .OrderByDescending(e => e.DataEmprestimo)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return (dados, total);
        }

        public async Task<Emprestimo?> BuscarPorIdAsync(int id)
        {
            return await _context.Emprestimos.FindAsync(id);
        }

        public async Task<Emprestimo> AdicionarAsync(Emprestimo emprestimo)
        {
            await _context.Emprestimos.AddAsync(emprestimo);
            await _context.SaveChangesAsync();
            return emprestimo;
        }

        public async Task AtualizarAsync(Emprestimo emprestimo)
        {
            _context.Emprestimos.Update(emprestimo);
            await _context.SaveChangesAsync();
        }

        public async Task<(List<Emprestimo> Dados, int Total)> BuscarAtivosAsync(int pagina, int tamanhoPagina)
        {
            var query = _context.Emprestimos
                .Where(e => !e.Devolvido);
            var total = await query.CountAsync();
            var dados = await query
                .OrderByDescending(e => e.DataEmprestimo)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return (dados, total);
        }

        public async Task<(List<Emprestimo> Dados, int Total)> BuscarInativosAsync(int pagina, int tamanhoPagina)
        {
            var query = _context.Emprestimos
                .Where(e => e.Devolvido);
            var total = await query.CountAsync();
            var dados = await query
                .OrderByDescending(e => e.DataDevolucao)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return (dados, total);
        }

        public async Task<(List<Emprestimo> Dados, int Total)> BuscarAtrasadosAsync(DateTime dataLimite, int pagina, int tamanhoPagina)
        {
            var query = _context.Emprestimos
                .Where(e => !e.Devolvido && e.DataPrevistaDevolucao < dataLimite);
            var total = await query.CountAsync();
            var dados = await query
                .OrderBy(e => e.DataPrevistaDevolucao)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();
            return (dados, total);
        }

        public async Task<List<Emprestimo>> BuscarAtrasadosSemMultaCalculadaAsync(int idCliente, DateTime dataLimite)
        {
            return await _context.Emprestimos
                .Where(e => e.IdCliente == idCliente
                    && !e.Devolvido
                    && e.DataPrevistaDevolucao < dataLimite
                    && e.ValorMulta == null)
                .ToListAsync();
        }

        public async Task<List<Emprestimo>> BuscarComMultaPendenteDeDescontoAsync(int idCliente)
        {
            return await _context.Emprestimos
                .Where(e => e.IdCliente == idCliente
                    && e.ValorMulta != null
                    && !e.MultaDescontada)
                .ToListAsync();
        }

        public async Task<bool> ExisteEmprestimoAtivoPorClienteAsync(int idCliente)
        {
            return await _context.Emprestimos
                .AnyAsync(e => e.IdCliente == idCliente && !e.Devolvido);
        }
    }
}