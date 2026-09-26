namespace EmprestimoLibrary.DTOs
{
    public record LoginResponseDto(
        string Token,
        string NomeCliente,
        string Email,
        DateTime Expiracao
        );
    
}
