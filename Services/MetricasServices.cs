using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Exceptions;
using EmprestimoLibrary.Repositories.interfaces;

namespace EmprestimoLibrary.Services
{
    public class MetricasService : IMetricasService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly ICarteiraRepository _carteiraRepository;
        private readonly ILivroRepository _livroRepository;

        public MetricasService(
            ICompraRepository compraRepository,
            ICarteiraRepository carteiraRepository,
            ILivroRepository livroRepository)
        {
            _compraRepository = compraRepository;
            _carteiraRepository = carteiraRepository;
            _livroRepository = livroRepository;
        }

        public async Task<MetricasResponseDto> ObterMetricasAsync(int idEditora)
        {
            // 1. Buscar carteira da editora
            var carteira = await _carteiraRepository.BuscarPorEditoraIdAsync(idEditora);

            if (carteira is null)
                throw new BusinessException("Carteira da editora não encontrada");

            // 2. Total de vendas
            var totalVendas = await _compraRepository.TotalVendasPorEditoraAsync(idEditora);

            // 3. Total de livros vendidos
            var totalLivrosVendidos = await _compraRepository.TotalLivrosVendidosPorEditoraAsync(idEditora);

            // 4. Total de transações
            var compras = await _compraRepository.BuscarPorEditoraIdAsync(idEditora);
            var totalTransacoes = compras.Count;

            // 5. Livros mais vendidos
            var livrosMaisVendidos = compras
                .GroupBy(c => c.IdLivro)
                .Select(g => new
                {
                    IdLivro = g.Key,
                    QuantidadeVendida = g.Sum(c => c.CompraQuantidade),
                    TotalArrecadado = g.Sum(c => c.CompraValorTotal)
                })
                .OrderByDescending(l => l.QuantidadeVendida)
                .Take(10) // top 10
                .ToList();

            // 6. Buscar títulos dos livros mais vendidos
            var livrosMaisVendidosDto = new List<LivroMaisVendidoDto>();

            foreach (var item in livrosMaisVendidos)
            {
                var livro = await _livroRepository.BuscarPorIdAsync(item.IdLivro);
                if (livro is not null)
                {
                    livrosMaisVendidosDto.Add(new LivroMaisVendidoDto(
                        livro.IdLivro,
                        livro.LivroTitulo,
                        item.QuantidadeVendida,
                        item.TotalArrecadado
                    ));
                }
            }

            return new MetricasResponseDto(
                totalVendas,
                totalLivrosVendidos,
                totalTransacoes,
                carteira.SaldoCarteira,
                livrosMaisVendidosDto
            );
        }
    }
}