namespace EmprestimoLibrary.Models
{
    public class Livro
    {
        public int IdLivro { get; set; }
        public string LivroTitulo { get; set; } = string.Empty;
        public string LivroAutor { get; set; } = string.Empty;
        public string LivroEditora { get; set; } = string.Empty;
        public string LivroEdicao { get; set; } = string.Empty;
        public int LivroQuantidade { get; set; }
        public decimal LivroPreco { get; set; }
        public string SituacaoLivro { get; set; } = "D";

        public bool IsEmprestavel { get; set; } = true;

        public int IdEditora { get; set; }
        public Editora? Editora { get; set; }
    }
}
