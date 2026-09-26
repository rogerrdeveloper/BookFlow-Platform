namespace EmprestimoLibrary.Models
{
    public class Emprestimo
    {
        public int IdEmprestimo { get; set; }
        public int IdCliente { get; set; }
        public int IdLivro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public bool Devolvido { get; set; }

        public decimal? ValorMulta { get; set; }

        public bool MultaDescontada { get; set; }
    }
}
