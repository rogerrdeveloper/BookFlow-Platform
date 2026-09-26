using EmprestimoLibrary.Models;

namespace EmprestimoLibrary.Repositories.interfaces
{
    public interface IClienteRepository
    {
        //Task significa operação assincrona,
        //List<Cliente> é o tipo de retorno
        Task<(List<Cliente> Dados, int Total)> BuscarTodosAsync(int pagina, int tamanho);
        Task<Cliente?> BuscarPorIdAsync(int id);
        Task<Cliente> AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task<bool> ExistePorCpfAsync(string cpf, int? idCliente = null);
        //CPF existe?
        //Se existe, ele pertence ao próprio cliente que estou editando?
        Task<bool> ExistePorEmailAsync(string email, int? idCliente = null);
        Task<Cliente?> BuscarPorEmailAsync(string email);
    }
}
