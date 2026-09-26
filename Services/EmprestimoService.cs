using EmprestimoLibrary.Constants;
using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Exceptions;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;

namespace EmprestimoLibrary.Services
{
    public class EmprestimoService : IEmprestimoService
    {
        private readonly IEmprestimoRepository _emprestimoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ILivroRepository _livroRepository;

        public EmprestimoService(
            IEmprestimoRepository emprestimoRepository,
            IClienteRepository clienteRepository,
            ILivroRepository livroRepository)
        {
            _emprestimoRepository = emprestimoRepository;
            _clienteRepository = clienteRepository;
            _livroRepository = livroRepository;
        }

        private EmprestimoResponseDto MapearResponseDto(Emprestimo emprestimo)
        {
            return new EmprestimoResponseDto(
                emprestimo.IdEmprestimo,
                emprestimo.IdCliente,
                emprestimo.IdLivro,
                emprestimo.DataEmprestimo,
                emprestimo.DataPrevistaDevolucao,
                emprestimo.DataDevolucao,
                emprestimo.Devolvido,
                emprestimo.ValorMulta,
                emprestimo.MultaDescontada
            );
        }

        private PaginaResponseDto<EmprestimoResponseDto> MontarPaginacao(
            List<Emprestimo> dados, int total, PaginacaoRequestDto paginacao)
        {
            return new PaginaResponseDto<EmprestimoResponseDto>(
                dados.Select(MapearResponseDto).ToList(),
                paginacao.Pagina,
                paginacao.TamanhoPagina,
                total,
                (int)Math.Ceiling((double)total / paginacao.TamanhoPagina)
            );
        }

        public async Task<PaginaResponseDto<EmprestimoResponseDto>> BuscarTodosAsync(PaginacaoRequestDto paginacao)
        {
            var (dados, total) = await _emprestimoRepository.BuscarTodosAsync(
                paginacao.Pagina, paginacao.TamanhoPagina);
            return MontarPaginacao(dados, total, paginacao);
        }

        public async Task<PaginaResponseDto<EmprestimoResponseDto>> BuscarAtivosAsync(PaginacaoRequestDto paginacao)
        {
            var (dados, total) = await _emprestimoRepository.BuscarAtivosAsync(
                paginacao.Pagina, paginacao.TamanhoPagina);
            return MontarPaginacao(dados, total, paginacao);
        }

        public async Task<PaginaResponseDto<EmprestimoResponseDto>> BuscarInativosAsync(PaginacaoRequestDto paginacao)
        {
            var (dados, total) = await _emprestimoRepository.BuscarInativosAsync(
                paginacao.Pagina, paginacao.TamanhoPagina);
            return MontarPaginacao(dados, total, paginacao);
        }

        public async Task<PaginaResponseDto<EmprestimoResponseDto>> BuscarAtrasadosAsync(PaginacaoRequestDto paginacao)
        {
            var (dados, total) = await _emprestimoRepository.BuscarAtrasadosAsync(
                DateTime.Now, paginacao.Pagina, paginacao.TamanhoPagina);
            return MontarPaginacao(dados, total, paginacao);
        }

        public async Task<EmprestimoResponseDto?> BuscarPorIdAsync(int id)
        {
            var emprestimo = await _emprestimoRepository.BuscarPorIdAsync(id);

            if (emprestimo is null)
                return null;

            return MapearResponseDto(emprestimo);
        }

        public async Task<EmprestimoResponseDto> AdicionarAsync(EmprestimoRequestDto emprestimoDto)
        {
            var cliente = await _clienteRepository.BuscarPorIdAsync(emprestimoDto.IdCliente);

            if (cliente is null)
                throw new BusinessException("Cliente não encontrado");

            if (!cliente.SituacaoConta)
                throw new BusinessException("Não é possível realizar empréstimo: conta desativada");

            var livro = await _livroRepository.BuscarPorIdAsync(emprestimoDto.IdLivro);

            if (livro is null)
                throw new BusinessException("Livro não encontrado.");

            if (livro.LivroQuantidade <= 0)
                throw new BusinessException("Livro indisponível para empréstimo.");

            livro.LivroQuantidade--;
            await _livroRepository.AtualizarAsync(livro);

            var dataEmprestimo = DateTime.Now;

            var emprestimo = new Emprestimo
            {
                IdCliente = emprestimoDto.IdCliente,
                IdLivro = emprestimoDto.IdLivro,
                DataEmprestimo = dataEmprestimo,
                DataPrevistaDevolucao = dataEmprestimo.AddDays(RegrasNegocio.PrazoEmprestimoDias),
                Devolvido = false
            };

            var emprestimoAdicionado = await _emprestimoRepository.AdicionarAsync(emprestimo);

            return MapearResponseDto(emprestimoAdicionado);
        }

        public async Task DevolverAsync(int id)
        {
            var emprestimo = await _emprestimoRepository.BuscarPorIdAsync(id);

            if (emprestimo is null)
                throw new BusinessException("Empréstimo não encontrado.");

            if (emprestimo.Devolvido)
                throw new BusinessException("Esse empréstimo já foi devolvido.");

            var livro = await _livroRepository.BuscarPorIdAsync(emprestimo.IdLivro);

            if (livro is null)
                throw new BusinessException("Livro não encontrado.");

            livro.LivroQuantidade++;
            await _livroRepository.AtualizarAsync(livro);

            var dataDevolucao = DateTime.Now;

            emprestimo.Devolvido = true;
            emprestimo.DataDevolucao = dataDevolucao;

            if (emprestimo.ValorMulta is null && dataDevolucao.Date > emprestimo.DataPrevistaDevolucao.Date)
            {
                int diasAtraso = (dataDevolucao.Date - emprestimo.DataPrevistaDevolucao.Date).Days;
                emprestimo.ValorMulta = diasAtraso * RegrasNegocio.ValorMultaPorDia;
            }

            await _emprestimoRepository.AtualizarAsync(emprestimo);
        }
    }
}