using EmprestimoLibrary.Data;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLibrary.Repositories
{
    public class EditoraRepository : IEditoraRepository
    {
        private readonly ControleEmprestimoLivroContext _context;

        public EditoraRepository(ControleEmprestimoLivroContext context)
        {
            _context = context;
        }
        public async Task<Editora> AdicionarAsync(Editora editora)
        {
            await _context.Editoras.AddAsync(editora);
            await _context.SaveChangesAsync();
            return editora;
        }

        public async Task AtualizarAsync(Editora editora)
        {
            _context.Editoras.Update(editora);
            await _context.SaveChangesAsync();
        }

        public async Task<Editora?> BuscarPorEmailAsync(string email)
        {
            return await _context.Editoras
                .FirstOrDefaultAsync(e => e.EmailEditora == email);
        }

        public async Task<Editora?> BuscarPorIdAsync(int id)
        {
            return await _context.Editoras.FindAsync(id);
        }

        public async Task<List<Editora>> BuscarTodosAsync()
        {
            return await _context.Editoras
                .OrderBy(e => e.NomeEditora)
                .ToListAsync();
        }

        public async Task<bool> ExistePorEmailAsync(string email, int? idEditora = null)
        {
            return await _context.Editoras
                .AnyAsync(e => e.EmailEditora == email &&
                        (idEditora == null || e.IdEditora != idEditora));
        }
    }
}
