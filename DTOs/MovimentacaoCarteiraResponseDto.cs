namespace EmprestimoLibrary.DTOs
{
    public record MovimentacaoCarteiraResponseDto(
        int IdMovimentacao,
        int IdCarteira,
        decimal ValorMovimento,
        string TipoMovimento,
        string Descricao,
        DateTime DataMovimento
        );
}
