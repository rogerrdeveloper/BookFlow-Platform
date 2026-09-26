namespace EmprestimoLibrary.DTOs
{
    public record LivroResponseDto(
         int IdLivro,
         string LivroTitulo,
         string LivroAutor,
         string LivroEditora,
         string LivroEdicao,
         int LivroQuantidade,
         decimal LivroPreco,
         string SituacaoLivro,
         bool IsEmprestavel
        );
}
