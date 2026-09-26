namespace EmprestimoLibrary.DTOs
{
    public record PaginaResponseDto<T>(
        List<T> Dados,
        int PaginaAtual,
        int TamanhoPagina,
        int TotalRegistros,
        int TotalPaginas
        );
}
