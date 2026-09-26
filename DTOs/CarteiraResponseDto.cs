namespace EmprestimoLibrary.DTOs
{
    public record CarteiraResponseDto(
        int IdCarteira,
        int? IdCliente,
        decimal SaldoCarteira
        );
}
