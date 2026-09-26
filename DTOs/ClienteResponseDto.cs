namespace EmprestimoLibrary.DTOs
{
    public record ClienteResponseDto(
        // feito como resposta da api pro usuario,
        // para que ele possa ver o id e informações do cliente que foi criado
        int IdCliente,
        string NomeCliente,
        string CpfCliente,
        string EnderecoCliente,
        string TelefoneCliente,
        string EmailCliente,
        bool SituacaoConta,
        decimal saldo);
}
