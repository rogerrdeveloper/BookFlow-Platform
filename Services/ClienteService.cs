using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Exceptions;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;

namespace EmprestimoLibrary.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEditoraRepository _editoraRepository;
        private readonly IEmprestimoRepository _emprestimoRepository;
        private readonly ICarteiraRepository _carteiraRepository;

        public ClienteService(
            IClienteRepository clienteRepository,
            IEditoraRepository editoraRepository,
            IEmprestimoRepository emprestimoRepository,
            ICarteiraRepository carteiraRepository)
        {
            _clienteRepository = clienteRepository;
            _editoraRepository = editoraRepository;
            _emprestimoRepository = emprestimoRepository;
            _carteiraRepository = carteiraRepository;
        }

        private async Task<ClienteResponseDto> MapearResponseDtoAsync(Cliente cliente)
        {
            var carteira = await _carteiraRepository.BuscarPorClienteIdAsync(cliente.IdCliente);
            var saldo = carteira?.SaldoCarteira ?? 0m;

            return new ClienteResponseDto(
                cliente.IdCliente,
                cliente.NomeCliente,
                cliente.CpfCliente,
                cliente.EnderecoCliente,
                cliente.TelefoneCliente,
                cliente.EmailCliente,
                cliente.SituacaoConta,
                saldo
            );
        }

        public async Task<PaginaResponseDto<ClienteResponseDto>> BuscarTodosAsync(PaginacaoRequestDto paginacao)
        {
            var (clientes, total) = await _clienteRepository.BuscarTodosAsync(
                paginacao.Pagina,
                paginacao.TamanhoPagina
            );

            var resultado = new List<ClienteResponseDto>();
            foreach (var cliente in clientes)
            {
                resultado.Add(await MapearResponseDtoAsync(cliente));
            }

            return new PaginaResponseDto<ClienteResponseDto>(
                resultado,
                paginacao.Pagina,
                paginacao.TamanhoPagina,
                total,
                (int)Math.Ceiling((double)total / paginacao.TamanhoPagina)
            );
        }

        public async Task<ClienteResponseDto?> BuscarPorIdAsync(int id)
        {
            var cliente = await _clienteRepository.BuscarPorIdAsync(id);

            if (cliente is null)
                return null;

            return await MapearResponseDtoAsync(cliente);
        }

        public async Task<ClienteResponseDto> AdicionarAsync(ClienteRequestDto clienteDto)
        {
            var cpfExistente = await _clienteRepository.ExistePorCpfAsync(clienteDto.CpfCliente);

            if (cpfExistente)
                throw new BusinessException("CPF já cadastrado");

            var emailExistenteCliente = await _clienteRepository.ExistePorEmailAsync(clienteDto.EmailCliente);

            if (emailExistenteCliente)
                throw new BusinessException("Email já cadastrado como cliente");

            // Verifica se já existe uma Editora com esse email para evitar conflito de carteiras e identidade
            var emailExistenteEditora = await _editoraRepository.ExistePorEmailAsync(clienteDto.EmailCliente);

            if (emailExistenteEditora)
                throw new BusinessException("Este email já está associado a uma conta de editora. Use um email diferente.");

            var cliente = new Cliente
            {
                NomeCliente = clienteDto.NomeCliente,
                CpfCliente = clienteDto.CpfCliente,
                EnderecoCliente = clienteDto.EnderecoCliente,
                TelefoneCliente = clienteDto.TelefoneCliente,
                EmailCliente = clienteDto.EmailCliente,
                SenhaCliente = BCrypt.Net.BCrypt.HashPassword(clienteDto.SenhaCliente),
                SituacaoConta = true
            };

            var clienteAdicionado = await _clienteRepository.AdicionarAsync(cliente);

            var carteira = new Carteira
            {
                IdCliente = clienteAdicionado.IdCliente,
                SaldoCarteira = 0m
            };
            await _carteiraRepository.AdicionarAsync(carteira);

            return await MapearResponseDtoAsync(clienteAdicionado);
        }

        public async Task AtualizarAsync(int id, ClienteAtualizarRequestDto clienteDto)
        {
            var cliente = await _clienteRepository.BuscarPorIdAsync(id);

            if (cliente is null)
                return;

            var cpfExiste = await _clienteRepository.ExistePorCpfAsync(clienteDto.CpfCliente, id);

            if (cpfExiste)
                throw new BusinessException("CPF já cadastrado para outro cliente");

            cliente.NomeCliente = clienteDto.NomeCliente;
            cliente.CpfCliente = clienteDto.CpfCliente;
            cliente.EnderecoCliente = clienteDto.EnderecoCliente;
            cliente.TelefoneCliente = clienteDto.TelefoneCliente;

            await _clienteRepository.AtualizarAsync(cliente);
        }

        public async Task DesativarContaAsync(int id)
        {
            var cliente = await _clienteRepository.BuscarPorIdAsync(id);

            if (cliente is null)
                throw new BusinessException("Cliente não encontrado");

            if (!cliente.SituacaoConta)
                throw new BusinessException("Conta já está cancelada.");

            var possuiEmprestimoAtivo = await _emprestimoRepository
                .ExisteEmprestimoAtivoPorClienteAsync(id);

            if (possuiEmprestimoAtivo)
                throw new BusinessException("Não é possível cancelar a conta: cliente com empréstimo ativo");

            var carteira = await _carteiraRepository.BuscarPorClienteIdAsync(id);

            if (carteira != null && carteira.SaldoCarteira > 0)
                throw new BusinessException("Saldo disponível na carteira: Saque antes de cancelar a conta");

            cliente.SituacaoConta = false;

            await _clienteRepository.AtualizarAsync(cliente);
        }
    }
}