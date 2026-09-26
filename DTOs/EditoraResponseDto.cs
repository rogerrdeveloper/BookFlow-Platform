namespace EmprestimoLibrary.DTOs
{
    public record EditoraResponseDto(
        int IdEditora,
        string NomeEditora,
        string EmailEditora,
        decimal SaldoCarteira
        );
}
