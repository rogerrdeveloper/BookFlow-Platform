namespace EmprestimoLibrary.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdCliente { get; set; }
        public int IdLivro { get; set; }
        public int CompraQuantidade { get; set; }

        public decimal CompraValorUnitario { get; set; }
        public decimal CompraValorTotal { get; set; }

        public DateTime DataCompra { get; set; }
    }
}
