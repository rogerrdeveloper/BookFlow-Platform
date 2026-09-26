using EmprestimoLibrary.Data;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLibrary.Repositories
{
    public class CarteiraRepository : ICarteiraRepository
    {
        private readonly ControleEmprestimoLivroContext _context;

        public CarteiraRepository(ControleEmprestimoLivroContext context)
        {
            _context = context;
        }
        public async Task<Carteira> AdicionarAsync(Carteira carteira)
        {
            await _context.Carteiras.AddAsync(carteira);
            await _context.SaveChangesAsync();
            return carteira;
        }

        public async Task AtualizarAsync(Carteira carteira)
        {
            _context.Carteiras.Update(carteira);
            await _context.SaveChangesAsync();
        }

        public async Task<Carteira?> BuscarPorClienteIdAsync(int idCliente)
        {
            return await _context.Carteiras.FirstOrDefaultAsync(c => c.IdCliente == idCliente);
        }

        public async Task<Carteira?> BuscarPorIdAsync(int id)
        {
            return await _context.Carteiras.FindAsync(id);
        }
        public async Task<Carteira?> BuscarPorEditoraIdAsync(int idEditora)
        {
            return await _context.Carteiras
                .FirstOrDefaultAsync(c => c.IdEditora == idEditora);
        }
    }
}
