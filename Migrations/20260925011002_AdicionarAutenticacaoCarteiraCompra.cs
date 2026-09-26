using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprestimoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarAutenticacaoCarteiraCompra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "livro_preco",
                table: "Livros",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "situacao_livro",
                table: "Livros",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "D");

            migrationBuilder.AddColumn<DateTime>(
                name: "lce_dataPrevistaDevolucao",
                table: "LIVRO_CLIENTE_EMPRESTIMO",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "lce_valorMulta",
                table: "LIVRO_CLIENTE_EMPRESTIMO",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email_cliente",
                table: "Clientes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "senha_cliente",
                table: "Clientes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "situacao_conta",
                table: "Clientes",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A");

            migrationBuilder.CreateTable(
                name: "Carteiras",
                columns: table => new
                {
                    id_carteira = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    saldo_carteira = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    ClienteIdCliente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carteiras", x => x.id_carteira);
                    table.ForeignKey(
                        name: "FK_Carteiras_Clientes_ClienteIdCliente",
                        column: x => x.ClienteIdCliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carteiras_Clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    id_compra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    id_livro = table.Column<int>(type: "int", nullable: false),
                    compra_quantidade = table.Column<int>(type: "int", nullable: false),
                    compra_valor_unitario = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    compra_valor_total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    data_compra = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.id_compra);
                    table.ForeignKey(
                        name: "FK_Compras_Clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "Clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compras_Livros_id_livro",
                        column: x => x.id_livro,
                        principalTable: "Livros",
                        principalColumn: "id_livro",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacoesCarteira",
                columns: table => new
                {
                    id_movimentacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_carteira = table.Column<int>(type: "int", nullable: false),
                    tipo_movimento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    valor_movimento = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    data_movimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacoesCarteira", x => x.id_movimentacao);
                    table.ForeignKey(
                        name: "FK_MovimentacoesCarteira_Carteiras_id_carteira",
                        column: x => x.id_carteira,
                        principalTable: "Carteiras",
                        principalColumn: "id_carteira",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_email_cliente",
                table: "Clientes",
                column: "email_cliente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carteiras_ClienteIdCliente",
                table: "Carteiras",
                column: "ClienteIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Carteiras_id_cliente",
                table: "Carteiras",
                column: "id_cliente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compras_id_cliente",
                table: "Compras",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_id_livro",
                table: "Compras",
                column: "id_livro");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacoesCarteira_id_carteira",
                table: "MovimentacoesCarteira",
                column: "id_carteira");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "MovimentacoesCarteira");

            migrationBuilder.DropTable(
                name: "Carteiras");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_email_cliente",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "livro_preco",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "situacao_livro",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "lce_dataPrevistaDevolucao",
                table: "LIVRO_CLIENTE_EMPRESTIMO");

            migrationBuilder.DropColumn(
                name: "lce_valorMulta",
                table: "LIVRO_CLIENTE_EMPRESTIMO");

            migrationBuilder.DropColumn(
                name: "email_cliente",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "senha_cliente",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "situacao_conta",
                table: "Clientes");
        }
    }
}
