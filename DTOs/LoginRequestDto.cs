using System.ComponentModel.DataAnnotations;

namespace EmprestimoLibrary.DTOs
{
    public record LoginRequestDto(
        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        string Email,
        [Required(ErrorMessage = "Senha é obrigatória")]
        string Senha
        );
}
