using System.ComponentModel.DataAnnotations;

namespace EmprestimoLibrary.DTOs
{
    public record EditoraRequestDto(
        [Required(ErrorMessage = "Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres.")]
        string NomeEditora,

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres.")]
        string EmailEditora,

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 255 caracteres.")]
        string SenhaEditora
    );
}