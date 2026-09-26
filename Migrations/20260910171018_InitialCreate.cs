using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprestimoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_cliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    cpf_cliente = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    endereco_cliente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    telefone_cliente = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "Livros",
                columns: table => new
                {
                    id_livro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    livro_titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    livro_autor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    livro_editora = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LivroEdicao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    livro_quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livros", x => x.id_livro);
                });

            migrationBuilder.CreateTable(
                name: "LIVRO_CLIENTE_EMPRESTIMO",
                columns: table => new
                {
                    id_emprestimo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    lce_id_cliente = table.Column<int>(type: "int", nullable: false),
                    lce_id_livro = table.Column<int>(type: "int", nullable: false),
                    lce_dataEmprestimo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lce_dataDevolucao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    lce_devolvido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LIVRO_CLIENTE_EMPRESTIMO", x => x.id_emprestimo);
                    table.ForeignKey(
                        name: "FK_LIVRO_CLIENTE_EMPRESTIMO_Clientes_lce_id_cliente",
                        column: x => x.lce_id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LIVRO_CLIENTE_EMPRESTIMO_Livros_lce_id_livro",
                        column: x => x.lce_id_livro,
                        principalTable: "Livros",
                        principalColumn: "id_livro",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_id_cliente",
                table: "Clientes",
                column: "id_cliente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LIVRO_CLIENTE_EMPRESTIMO_lce_id_cliente",
                table: "LIVRO_CLIENTE_EMPRESTIMO",
                column: "lce_id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_LIVRO_CLIENTE_EMPRESTIMO_lce_id_livro",
                table: "LIVRO_CLIENTE_EMPRESTIMO",
                column: "lce_id_livro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LIVRO_CLIENTE_EMPRESTIMO");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Livros");
        }
    }
}
