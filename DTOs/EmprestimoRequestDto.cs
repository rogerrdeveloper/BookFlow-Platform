using System.ComponentModel.DataAnnotations;

namespace EmprestimoLibrary.DTOs
{
    public record EmprestimoRequestDto(
        [Range(1, int.MaxValue,
            ErrorMessage = "Cliente inválido.")]
        int IdCliente,
        [Range(1, int.MaxValue,
            ErrorMessage = "Livro inválido.")]
        int IdLivro
        );
}
