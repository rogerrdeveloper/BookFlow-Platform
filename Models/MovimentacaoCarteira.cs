namespace EmprestimoLibrary.Models
{
    public class MovimentacaoCarteira
    {
        public int IdMovimentacao { get; set; }
        public int IdCarteira { get; set; } //FK

        public string TipoMovimento { get; set; } = string.Empty;
        // D = Deposito, S = Saque, C = Compra, M = Multa
        public decimal ValorMovimento { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataMovimento { get; set; }

    }
}
