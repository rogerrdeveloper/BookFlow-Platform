namespace EmprestimoLibrary.Models
{
    public class Carteira
    {
        public int IdCarteira { get; set; } // PK 
        public int? IdCliente { get; set; } // FK 
        public int? IdEditora { get; set; } // FK 
        public Cliente? Cliente { get; set; }  // 
        public Editora? Editora { get; set; }

        public decimal SaldoCarteira { get; set; }

        public ICollection<MovimentacaoCarteira> Movimentacoes { get; set; }
            = new List<MovimentacaoCarteira>();
    }
}