namespace EmprestimoLibrary.Models
{
    public class Editora
    {
        public int IdEditora { get; set; }
        public string NomeEditora { get; set; } = string.Empty;
        public string EmailEditora { get; set; } = string.Empty;
        public string SenhaEditora { get; set; } = string.Empty;

        public Carteira? Carteira { get; set; }
    }
}
