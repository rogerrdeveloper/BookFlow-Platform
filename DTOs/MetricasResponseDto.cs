namespace EmprestimoLibrary.DTOs
{
    public record MetricasResponseDto(
        decimal TotalVendas,
        int TotalLivrosVendidos,
        int TotalTransacoes,
        decimal SaldoCarteira,
        List<LivroMaisVendidoDto> LivrosMaisVendidos
    );

    public record LivroMaisVendidoDto(
        int IdLivro,
        string Titulo,
        int QuantidadeVendida,
        decimal TotalArrecadado
    );
}
