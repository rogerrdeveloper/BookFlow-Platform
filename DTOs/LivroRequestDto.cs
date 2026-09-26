using System.ComponentModel.DataAnnotations;

namespace EmprestimoLibrary.DTOs
{
    public record LivroRequestDto(
         [Required]
         [StringLength(200)]
         string LivroTitulo,
         [Required]
         [StringLength(200)]
         string LivroAutor,
         [Required]
         [StringLength(200)]
         string LivroEditora,
         [Required]
         [StringLength(50)]
         string LivroEdicao,
         [Range(0, int.MaxValue, ErrorMessage = "A quantidade não pode ser negativo.")]
         int LivroQuantidade,
         [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
         decimal LivroPreco,
         bool IsEmprestavel = true
    );
}
