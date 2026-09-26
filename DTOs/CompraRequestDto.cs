using System.ComponentModel.DataAnnotations;

namespace EmprestimoLibrary.DTOs
{
    public record CompraRequestDto(
        [Range(1, int.MaxValue, ErrorMessage = "Cliente inválido")]
        int IdCliente,
        [Range(1, int.MaxValue, ErrorMessage = "Livro inválido.")]
        int IdLivro,
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser no mínimo 1.")]
        int CompraQuantidade
    );
}
