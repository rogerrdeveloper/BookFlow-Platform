using System.ComponentModel.DataAnnotations;

namespace EmprestimoLibrary.DTOs
{
    public record CarteiraOperacaoRequestDto(
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        decimal Valor,
        string? Descricao = null
    );
}
