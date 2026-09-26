using EmprestimoLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLibrary.Data
{
    public class ControleEmprestimoLivroContext : DbContext
    {
        public ControleEmprestimoLivroContext (DbContextOptions<ControleEmprestimoLivroContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Carteira> Carteiras { get; set; }
        public DbSet<MovimentacaoCarteira> Movimentacoes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Editora> Editoras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.Property(e => e.IdCliente)
                    .HasColumnName("id_cliente");

                entity.Property(e => e.NomeCliente)
                    .HasColumnName("nome_cliente")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CpfCliente)
                    .HasColumnName("cpf_cliente")
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.EnderecoCliente)
                    .HasColumnName("endereco_cliente")
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.TelefoneCliente)
                    .HasColumnName("telefone_cliente")
                    .IsRequired()
                    .HasMaxLength(14);

                entity.Property(e => e.EmailCliente)
                    .HasColumnName("email_cliente")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SenhaCliente)
                    .HasColumnName("senha_cliente")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.SituacaoConta)
                    .HasColumnName("situacao_conta")
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.IdCliente)
                    .IsUnique();

                entity.HasIndex(e => e.EmailCliente)
                    .IsUnique();
            });

            modelBuilder.Entity<Livro>(entity =>
            {
                entity.ToTable("Livros");

                entity.HasKey(e => e.IdLivro);

                entity.Property(e => e.IdLivro)
                    .HasColumnName("id_livro");

                entity.Property(e => e.LivroTitulo)
                    .HasColumnName("livro_titulo")
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LivroAutor)
                    .HasColumnName("livro_autor")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LivroEditora)
                    .HasColumnName("livro_editora")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LivroEdicao)
                    .HasColumnName("livro_edicao")
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LivroQuantidade)
                    .HasColumnName("livro_quantidade")
                    .IsRequired();

                entity.Property(e => e.LivroPreco)
                    .HasColumnName("livro_preco")
                    .IsRequired()
                    .HasPrecision(10, 2);

                entity.Property(e => e.SituacaoLivro)
                    .HasColumnName("situacao_livro")
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasDefaultValue("D");

                entity.Property(e => e.IsEmprestavel)
                .HasColumnName("is_emprestavel")
                .IsRequired()
                 .HasDefaultValue(true);

                entity.Property(e => e.IdEditora)
                    .HasColumnName("id_editora")
                    .IsRequired();

                entity.HasOne<Editora>()
                    .WithMany()
                    .HasForeignKey(e => e.IdEditora)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.ToTable("LIVRO_CLIENTE_EMPRESTIMO");

                entity.HasKey(e => e.IdEmprestimo);

                entity.Property(e => e.IdEmprestimo)
                    .HasColumnName("id_emprestimo");

                entity.Property(e => e.IdCliente)
                    .HasColumnName("lce_id_cliente")
                    .IsRequired();

                entity.Property(e => e.IdLivro)
                    .HasColumnName("lce_id_livro")
                    .IsRequired();

                entity.Property(e => e.DataEmprestimo)
                    .HasColumnName("lce_dataEmprestimo")
                    .IsRequired();

                entity.Property(e => e.DataPrevistaDevolucao)
                    .HasColumnName("lce_dataPrevistaDevolucao")
                    .IsRequired();

                entity.Property(e => e.DataDevolucao)
                    .HasColumnName("lce_dataDevolucao");

                entity.Property(e => e.Devolvido)
                    .HasColumnName("lce_devolvido")
                    .IsRequired();

                entity.Property(e => e.ValorMulta)
                    .HasColumnName("lce_valorMulta")
                    .HasPrecision(10, 2);
                entity.Property(e => e.MultaDescontada)
                    .HasColumnName("lce_multaDescontada")
                    .IsRequired()
                    .HasDefaultValue(false);

                // RELACIONAMENTOS
                entity.HasOne<Cliente>()
                    .WithMany()
                    .HasForeignKey(e => e.IdCliente)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Livro>()
                    .WithMany()
                    .HasForeignKey(e => e.IdLivro)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Carteira>(entity =>
            {
                entity.ToTable("Carteiras");

                entity.HasKey(e => e.IdCarteira);

                entity.Property(e => e.IdCarteira)
                    .HasColumnName("id_carteira");

                entity.Property(e => e.IdCliente)
                    .HasColumnName("id_cliente");

                entity.Property(e => e.IdEditora)
                    .HasColumnName("id_editora");

                entity.Property(e => e.SaldoCarteira)
                    .HasColumnName("saldo_carteira")
                    .IsRequired()
                    .HasPrecision(10, 2);

                // Constraint — cada carteira tem exatamente um dono
                entity.ToTable("Carteiras", t => t.HasCheckConstraint(
                    "CK_Carteira_Dono",
                    "(id_cliente IS NOT NULL AND id_editora IS NULL) OR (id_cliente IS NULL AND id_editora IS NOT NULL)"
                    ));

                // Índice único — um cliente só pode ter uma carteira
                entity.HasIndex(e => e.IdCliente)
                    .IsUnique()
                    .HasFilter("[id_cliente] IS NOT NULL");

                // Índice único — uma editora só pode ter uma carteira
                entity.HasIndex(e => e.IdEditora)
                    .IsUnique()
                    .HasFilter("[id_editora] IS NOT NULL");

                entity.HasOne<Cliente>()
                    .WithOne(c => c.Carteira)
                    .HasForeignKey<Carteira>(e => e.IdCliente)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Editora>()
                    .WithOne(e => e.Carteira)
                    .HasForeignKey<Carteira>(e => e.IdEditora)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany<MovimentacaoCarteira>()
                    .WithOne()
                    .HasForeignKey(e => e.IdCarteira)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MovimentacaoCarteira>(entity =>
            {
                entity.ToTable("MovimentacoesCarteira");

                entity.HasKey(e => e.IdMovimentacao);

                entity.Property(e => e.IdMovimentacao)
                    .HasColumnName("id_movimentacao");

                entity.Property(e => e.IdCarteira)
                    .HasColumnName("id_carteira")
                    .IsRequired();

                entity.Property(e => e.ValorMovimento)
                    .HasColumnName("valor_movimento")
                    .IsRequired()
                    .HasPrecision(10, 2);

                entity.Property(e => e.TipoMovimento)
                    .HasColumnName("tipo_movimento")
                    .IsRequired()
                    .HasMaxLength(20);  // "DEPOSITO", "SAQUE", "MULTA", "COMPRA"

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasMaxLength(255);

                entity.Property(e => e.DataMovimento)
                    .HasColumnName("data_movimento")
                    .IsRequired();

                entity.HasOne<Carteira>()
                    .WithMany(c => c.Movimentacoes)
                    .HasForeignKey(e => e.IdCarteira)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Compra>(entity =>
            {
                entity.ToTable("Compras");

                entity.HasKey(e => e.IdCompra);

                entity.Property(e => e.IdCompra)
                    .HasColumnName("id_compra");

                entity.Property(e => e.IdCliente)
                    .HasColumnName("id_cliente")
                    .IsRequired();

                entity.Property(e => e.IdLivro)
                    .HasColumnName("id_livro")
                    .IsRequired();

                entity.Property(e => e.CompraQuantidade)
                    .HasColumnName("compra_quantidade")
                    .IsRequired();

                entity.Property(e => e.CompraValorUnitario)
                    .HasColumnName("compra_valor_unitario")
                    .IsRequired()
                    .HasPrecision(10, 2);

                entity.Property(e => e.CompraValorTotal)
                    .HasColumnName("compra_valor_total")
                    .IsRequired()
                    .HasPrecision(10, 2);

                entity.Property(e => e.DataCompra)
                    .HasColumnName("data_compra")
                    .IsRequired();

                entity.HasOne<Cliente>()
                    .WithMany()
                    .HasForeignKey(e => e.IdCliente)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne<Livro>()
                    .WithMany()
                    .HasForeignKey(e => e.IdLivro)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Editora>(entity =>
            {
                entity.ToTable("Editoras");

                entity.HasKey(e => e.IdEditora);

                entity.Property(e => e.IdEditora)
                    .HasColumnName("id_editora");

                entity.Property(e => e.NomeEditora)
                    .HasColumnName("nome_editora")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EmailEditora)
                    .HasColumnName("email_editora")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SenhaEditora)
                    .HasColumnName("senha_editora")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasIndex(e => e.EmailEditora)
                    .IsUnique();
            });
        }

    }
}
