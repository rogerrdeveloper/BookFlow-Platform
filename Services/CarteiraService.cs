using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Exceptions;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;

namespace EmprestimoLibrary.Services
{
    public class CarteiraService : ICarteiraService
    {
        private readonly ICarteiraRepository _carteiraRepository;
        private readonly IMovimentacaoCarteiraRepository _movimentacaoRepository;
        private readonly IEmprestimoRepository _emprestimoRepository;

        public CarteiraService(
            ICarteiraRepository carteiraRepository,
            IMovimentacaoCarteiraRepository movimentacaoRepository,
            IEmprestimoRepository emprestimoRepository)
        {
            _carteiraRepository = carteiraRepository;
            _movimentacaoRepository = movimentacaoRepository;
            _emprestimoRepository = emprestimoRepository;
        }

        public async Task<CarteiraResponseDto?> BuscarPorClienteIdAsync(int idCliente)
        {
            var carteira = await _carteiraRepository.BuscarPorClienteIdAsync(idCliente);

            if (carteira is null)
                return null;

            return MapearResponseDto(carteira);
        }

        public async Task<CarteiraResponseDto> DepositarAsync(int idCliente, CarteiraOperacaoRequestDto operacao)
        {
            var carteira = await _carteiraRepository.BuscarPorClienteIdAsync(idCliente);

            if (carteira is null)
                throw new BusinessException($"Carteira não encontrada para o cliente id: {idCliente}");

            if (operacao.Valor <= 0)
                throw new BusinessException("O valor depositado precisa ser maior que 0");

            carteira.SaldoCarteira += operacao.Valor;
            await _carteiraRepository.AtualizarAsync(carteira);

            var movimentacao = new MovimentacaoCarteira
            {
                IdCarteira = carteira.IdCarteira,
                TipoMovimento = "DEPOSITO",
                ValorMovimento = operacao.Valor,
                Descricao = operacao.Descricao ?? "Depósito na carteira",
                DataMovimento = DateTime.Now
            };
            await _movimentacaoRepository.AdicionarAsync(movimentacao);

            return MapearResponseDto(carteira);
        }

        public async Task<CarteiraResponseDto> SacarAsync(int idCliente, CarteiraOperacaoRequestDto operacao)
        {
            var carteira = await _carteiraRepository.BuscarPorClienteIdAsync(idCliente);

            if (carteira is null)
                throw new BusinessException($"Carteira não encontrada para cliente {idCliente}");

            if (operacao.Valor <= 0)
                throw new BusinessException("Valor de saque deve ser maior que zero");

            if (carteira.SaldoCarteira - operacao.Valor < 0)
                throw new BusinessException($"Saldo insuficiente. Saldo atual: {carteira.SaldoCarteira:C}, tentativa de saque: {operacao.Valor:C}");

            carteira.SaldoCarteira -= operacao.Valor;
            await _carteiraRepository.AtualizarAsync(carteira);

            var movimentacao = new MovimentacaoCarteira
            {
                IdCarteira = carteira.IdCarteira,
                TipoMovimento = "SAQUE",
                ValorMovimento = operacao.Valor,
                Descricao = operacao.Descricao ?? "Saque da carteira",
                DataMovimento = DateTime.Now
            };
            await _movimentacaoRepository.AdicionarAsync(movimentacao);

            return MapearResponseDto(carteira);
        }

        public async Task<PaginaResponseDto<MovimentacaoCarteiraResponseDto>> ObterExtratoAsync(int idCliente, PaginacaoRequestDto paginacao)
        {
            var carteira = await _carteiraRepository.BuscarPorClienteIdAsync(idCliente);

            if (carteira is null)
                throw new BusinessException($"Carteira não encontrada para cliente {idCliente}");

            var (movimentacoes, total) = await _movimentacaoRepository.BuscarPorCarteiraAsync(
                carteira.IdCarteira,
                paginacao.Pagina,
                paginacao.TamanhoPagina
            );

            return new PaginaResponseDto<MovimentacaoCarteiraResponseDto>(
                movimentacoes.Select(MapearMovimentacaoResponseDto).ToList(),
                paginacao.Pagina,
                paginacao.TamanhoPagina,
                total,
                (int)Math.Ceiling((double)total / paginacao.TamanhoPagina)
            );
        }

        public async Task CobrarMultasPendentesAsync(int idCliente)
        {
            var carteira = await _carteiraRepository.BuscarPorClienteIdAsync(idCliente);

            if (carteira is null)
                throw new BusinessException($"Carteira não encontrada para cliente {idCliente}");

            var emprestimosComMulta = await _emprestimoRepository
                .BuscarComMultaPendenteDeDescontoAsync(idCliente);

            foreach (var emprestimo in emprestimosComMulta)
            {
                if (emprestimo.ValorMulta.HasValue && emprestimo.ValorMulta > 0)
                {
                    carteira.SaldoCarteira -= emprestimo.ValorMulta.Value;
                    await _carteiraRepository.AtualizarAsync(carteira);

                    emprestimo.MultaDescontada = true;
                    await _emprestimoRepository.AtualizarAsync(emprestimo);

                    var movimentacao = new MovimentacaoCarteira
                    {
                        IdCarteira = carteira.IdCarteira,
                        TipoMovimento = "MULTA",
                        ValorMovimento = emprestimo.ValorMulta.Value,
                        Descricao = $"Multa por atraso na devolução do empréstimo #{emprestimo.IdEmprestimo}",
                        DataMovimento = DateTime.Now
                    };
                    await _movimentacaoRepository.AdicionarAsync(movimentacao);
                }
            }
        }

        public async Task<CarteiraResponseDto?> BuscarPorEditoraIdAsync(int idEditora)
        {
            var carteira = await _carteiraRepository.BuscarPorEditoraIdAsync(idEditora);

            if (carteira is null)
                return null;

            return MapearResponseDto(carteira);
        }

        public async Task<CarteiraResponseDto> SacarEditoraAsync(int idEditora, CarteiraOperacaoRequestDto operacao)
        {
            var carteira = await _carteiraRepository.BuscarPorEditoraIdAsync(idEditora);

            if (carteira is null)
                throw new BusinessException($"Carteira não encontrada para editora {idEditora}");

            if (operacao.Valor <= 0)
                throw new BusinessException("Valor de saque deve ser maior que zero");

            if (carteira.SaldoCarteira - operacao.Valor < 0)
                throw new BusinessException($"Saldo insuficiente na editora. Saldo atual: {carteira.SaldoCarteira:C}, tentativa de saque: {operacao.Valor:C}");

            carteira.SaldoCarteira -= operacao.Valor;
            await _carteiraRepository.AtualizarAsync(carteira);

            var movimentacao = new MovimentacaoCarteira
            {
                IdCarteira = carteira.IdCarteira,
                TipoMovimento = "SAQUE",
                ValorMovimento = operacao.Valor,
                Descricao = operacao.Descricao ?? "Saque de receitas de vendas (Editora)",
                DataMovimento = DateTime.Now
            };
            await _movimentacaoRepository.AdicionarAsync(movimentacao);

            return MapearResponseDto(carteira);
        }

        public async Task<PaginaResponseDto<MovimentacaoCarteiraResponseDto>> ObterExtratoEditoraAsync(int idEditora, PaginacaoRequestDto paginacao)
        {
            var carteira = await _carteiraRepository.BuscarPorEditoraIdAsync(idEditora);

            if (carteira is null)
                throw new BusinessException($"Carteira não encontrada para editora {idEditora}");

            var (movimentacoes, total) = await _movimentacaoRepository.BuscarPorCarteiraAsync(
                carteira.IdCarteira,
                paginacao.Pagina,
                paginacao.TamanhoPagina
            );

            return new PaginaResponseDto<MovimentacaoCarteiraResponseDto>(
                movimentacoes.Select(MapearMovimentacaoResponseDto).ToList(),
                paginacao.Pagina,
                paginacao.TamanhoPagina,
                total,
                (int)Math.Ceiling((double)total / paginacao.TamanhoPagina)
            );
        }

        private CarteiraResponseDto MapearResponseDto(Carteira carteira)
        {
            return new CarteiraResponseDto(
                carteira.IdCarteira,
                carteira.IdCliente,
                carteira.SaldoCarteira
            );
        }

        private MovimentacaoCarteiraResponseDto MapearMovimentacaoResponseDto(MovimentacaoCarteira movimentacao)
        {
            return new MovimentacaoCarteiraResponseDto(
                movimentacao.IdMovimentacao,
                movimentacao.IdCarteira,
                movimentacao.ValorMovimento,
                movimentacao.TipoMovimento,
                movimentacao.Descricao,
                movimentacao.DataMovimento
            );
        }
    }
}