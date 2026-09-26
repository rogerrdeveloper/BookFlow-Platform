using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;

namespace EmprestimoLibrary.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ILivroRepository _livroRepository;
        private readonly ICarteiraRepository _carteiraRepository;
        private readonly IMovimentacaoCarteiraRepository _movimentacaoRepository;

        public CompraService(
            ICompraRepository compraRepository,
            IClienteRepository clienteRepository,
            ILivroRepository livroRepository,
            ICarteiraRepository carteiraRepository,
            IMovimentacaoCarteiraRepository movimentacaoRepository)
        {
            _compraRepository = compraRepository;
            _clienteRepository = clienteRepository;
            _livroRepository = livroRepository;
            _carteiraRepository = carteiraRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<PaginaResponseDto<CompraResponseDto>> BuscarPorClienteIdAsync(int idCliente, PaginacaoRequestDto paginacao)
        {
            var (compras, total) = await _compraRepository.BuscarPorClienteIdAsync(
                idCliente,
                paginacao.Pagina,
                paginacao.TamanhoPagina
            );

            return new PaginaResponseDto<CompraResponseDto>(
                compras.Select(MapearResponseDto).ToList(),
                paginacao.Pagina,
                paginacao.TamanhoPagina,
                total,
                (int)Math.Ceiling((double)total / paginacao.TamanhoPagina)
            );
        }

        public async Task<CompraResponseDto?> BuscarPorIdAsync(int idCompra)
        {
            var compra = await _compraRepository.BuscarPorIdAsync(idCompra);

            if (compra is null)
                return null;

            return MapearResponseDto(compra);
        }

        public async Task<CompraResponseDto> ComprarAsync(CompraRequestDto compraDto)
        {
            var cliente = await _clienteRepository.BuscarPorIdAsync(compraDto.IdCliente);

            if (cliente is null)
                throw new InvalidOperationException($"Cliente {compraDto.IdCliente} não encontrado");

            if (!cliente.SituacaoConta)
                throw new InvalidOperationException("Não é possível fazer compra. Conta do cliente está inativa");

            var livro = await _livroRepository.BuscarPorIdAsync(compraDto.IdLivro);

            if (livro is null)
                throw new InvalidOperationException($"Livro {compraDto.IdLivro} não encontrado");

            if (livro.LivroQuantidade <= 0)
                throw new InvalidOperationException($"Livro '{livro.LivroTitulo}' não está disponível em estoque");

            if (compraDto.CompraQuantidade > livro.LivroQuantidade)
                throw new InvalidOperationException("A quantidade deve ser menor ou igual ao estoque disponível");

            var carteiraCliente = await _carteiraRepository.BuscarPorClienteIdAsync(compraDto.IdCliente);

            if (carteiraCliente is null)
                throw new InvalidOperationException($"Carteira não encontrada para cliente {compraDto.IdCliente}");

            var valorUnitario = livro.LivroPreco;
            var valorTotal = valorUnitario * compraDto.CompraQuantidade;

            if (carteiraCliente.SaldoCarteira < valorTotal)
                throw new InvalidOperationException(
                    $"Saldo insuficiente. Saldo atual: {carteiraCliente.SaldoCarteira:C}, custo da compra: {valorTotal:C}");

            var carteiraEditora = await _carteiraRepository.BuscarPorEditoraIdAsync(livro.IdEditora);

            if (carteiraEditora is null)
                throw new InvalidOperationException("Carteira da editora não encontrada");

            var compra = new Compra
            {
                IdCliente = compraDto.IdCliente,
                IdLivro = compraDto.IdLivro,
                CompraQuantidade = compraDto.CompraQuantidade,
                CompraValorUnitario = valorUnitario,
                CompraValorTotal = valorTotal,
                DataCompra = DateTime.Now
            };

            await _compraRepository.AdicionarAsync(compra);

            livro.LivroQuantidade -= compraDto.CompraQuantidade;
            await _livroRepository.AtualizarAsync(livro);

            carteiraCliente.SaldoCarteira -= valorTotal;
            await _carteiraRepository.AtualizarAsync(carteiraCliente);

            var movimentacaoCliente = new MovimentacaoCarteira
            {
                IdCarteira = carteiraCliente.IdCarteira,
                TipoMovimento = "COMPRA",
                ValorMovimento = valorTotal,
                Descricao = $"Compra de {compraDto.CompraQuantidade}x '{livro.LivroTitulo}'",
                DataMovimento = DateTime.Now
            };
            await _movimentacaoRepository.AdicionarAsync(movimentacaoCliente);

            carteiraEditora.SaldoCarteira += valorTotal;
            await _carteiraRepository.AtualizarAsync(carteiraEditora);

            var movimentacaoEditora = new MovimentacaoCarteira
            {
                IdCarteira = carteiraEditora.IdCarteira,
                TipoMovimento = "VENDA",
                ValorMovimento = valorTotal,
                Descricao = $"Venda de {compraDto.CompraQuantidade}x '{livro.LivroTitulo}'",
                DataMovimento = DateTime.Now
            };
            await _movimentacaoRepository.AdicionarAsync(movimentacaoEditora);

            return MapearResponseDto(compra);
        }

        private CompraResponseDto MapearResponseDto(Compra compra)
        {
            return new CompraResponseDto(
                compra.IdCompra,
                compra.IdCliente,
                compra.IdLivro,
                compra.CompraQuantidade,
                compra.CompraValorUnitario,
                compra.CompraValorTotal,
                compra.DataCompra
            );
        }
    }
}