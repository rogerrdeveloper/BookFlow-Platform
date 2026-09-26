namespace EmprestimoLibrary.DTOs
{
    public record EmprestimoResponseDto(
        int IdEmprestimo,
        int IdCliente,
        int IdLivro,
        DateTime DataEmprestimo,
        DateTime DataPrevistaDevolucao,
        DateTime? DataDevolucao,
        bool Devolvido,
        decimal? ValorMulta,
        bool MultaDescontada
    );
}
