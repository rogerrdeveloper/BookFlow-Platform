using System.ComponentModel.DataAnnotations;

namespace EmprestimoLibrary.DTOs
{
    public record PaginacaoRequestDto(
        [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que zero.")]
        int Pagina = 1,

        [Range(1, 50, ErrorMessage = "Tamanho máximo por página é 50.")]
        int TamanhoPagina = 10,

        string? Titulo = null
        );
}
