using EmprestimoLibrary.Data;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLibrary.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ControleEmprestimoLivroContext _context;

        public ClienteRepository(ControleEmprestimoLivroContext context)
        {
            _context = context;
        }

        public async Task<(List<Cliente> Dados, int Total)> BuscarTodosAsync(int pagina, int tamanhoPagina)
        {
            var query = _context.Clientes.AsQueryable();

            var total = await query.CountAsync();

            var dados = await query
                .OrderBy(c => c.NomeCliente)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (dados, total);
        }

        public async Task<Cliente?> BuscarPorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Cliente> AdicionarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<Cliente?> BuscarPorEmailAsync(string email)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.EmailCliente == email);
        }

        public async Task<bool> ExistePorCpfAsync(string cpf, int? idCliente = null)
        {
            return await _context.Clientes
                .AnyAsync(c => c.CpfCliente == cpf &&
                (idCliente == null || c.IdCliente != idCliente));
        }

        public async Task<bool> ExistePorEmailAsync(string email, int? idCliente = null)
        {
            return await _context.Clientes
                .AnyAsync(c => c.EmailCliente == email &&
                (idCliente == null || c.IdCliente != idCliente));
        }
    }
}