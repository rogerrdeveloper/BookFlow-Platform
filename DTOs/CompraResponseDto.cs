namespace EmprestimoLibrary.DTOs
{
    public record CompraResponseDto(
        int IdCompra,
        int IdCliente,
        int IdLivro,
        int CompraQuantidade,
        decimal CompraValorUnitario,
        decimal CompraValorTotal,
        DateTime DataCompra
        );
}
