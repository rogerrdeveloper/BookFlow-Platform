using EmprestimoLibrary.Data;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLibrary.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private readonly ControleEmprestimoLivroContext _context;

        public LivroRepository(ControleEmprestimoLivroContext context)
        {
            _context = context;
        }

        public async Task<(List<Livro> Dados, int Total)> BuscarTodosAsync(int pagina, int tamanhoPagina, string? titulo = null)
        {
            var query = _context.Livros.AsQueryable();

            if (!string.IsNullOrWhiteSpace(titulo))
                query = query.Where(l => l.LivroTitulo.Contains(titulo));

            var total = await query.CountAsync();

            var dados = await query
                .OrderBy(l => l.LivroTitulo)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (dados, total);
        }

        public async Task<(List<Livro> Dados, int Total)> BuscarPorEditoraAsync(int idEditora, int pagina, int tamanhoPagina)
        {
            var query = _context.Livros.Where(l => l.IdEditora == idEditora);

            var total = await query.CountAsync();

            var dados = await query
                .OrderBy(l => l.LivroTitulo)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (dados, total);
        }

        public async Task<Livro?> BuscarPorIdAsync(int id)
        {
            return await _context.Livros.FindAsync(id);
        }

        public async Task<Livro> AdicionarAsync(Livro livro)
        {
            await _context.Livros.AddAsync(livro);
            await _context.SaveChangesAsync();
            return livro;
        }

        public async Task AtualizarAsync(Livro livro)
        {
            _context.Livros.Update(livro);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(Livro livro)
        {
            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();
        }
    }
}