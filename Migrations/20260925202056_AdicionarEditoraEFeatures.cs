using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprestimoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEditoraEFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carteiras_Clientes_ClienteIdCliente",
                table: "Carteiras");

            migrationBuilder.DropIndex(
                name: "IX_Carteiras_id_cliente",
                table: "Carteiras");

            migrationBuilder.AddColumn<int>(
                name: "EditoraIdEditora",
                table: "Livros",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_editora",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_emprestavel",
                table: "Livros",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "id_cliente",
                table: "Carteiras",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteIdCliente",
                table: "Carteiras",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EditoraIdEditora",
                table: "Carteiras",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_editora",
                table: "Carteiras",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Editoras",
                columns: table => new
                {
                    id_editora = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome_editora = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email_editora = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    senha_editora = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editoras", x => x.id_editora);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livros_EditoraIdEditora",
                table: "Livros",
                column: "EditoraIdEditora");

            migrationBuilder.CreateIndex(
                name: "IX_Livros_id_editora",
                table: "Livros",
                column: "id_editora");

            migrationBuilder.CreateIndex(
                name: "IX_Carteiras_EditoraIdEditora",
                table: "Carteiras",
                column: "EditoraIdEditora");

            migrationBuilder.CreateIndex(
                name: "IX_Carteiras_id_cliente",
                table: "Carteiras",
                column: "id_cliente",
                unique: true,
                filter: "[id_cliente] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Carteiras_id_editora",
                table: "Carteiras",
                column: "id_editora",
                unique: true,
                filter: "[id_editora] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Carteira_Dono",
                table: "Carteiras",
                sql: "(id_cliente IS NOT NULL AND id_editora IS NULL) OR (id_cliente IS NULL AND id_editora IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Editoras_email_editora",
                table: "Editoras",
                column: "email_editora",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carteiras_Clientes_ClienteIdCliente",
                table: "Carteiras",
                column: "ClienteIdCliente",
                principalTable: "Clientes",
                principalColumn: "id_cliente");

            migrationBuilder.AddForeignKey(
                name: "FK_Carteiras_Editoras_EditoraIdEditora",
                table: "Carteiras",
                column: "EditoraIdEditora",
                principalTable: "Editoras",
                principalColumn: "id_editora");

            migrationBuilder.AddForeignKey(
                name: "FK_Carteiras_Editoras_id_editora",
                table: "Carteiras",
                column: "id_editora",
                principalTable: "Editoras",
                principalColumn: "id_editora",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Editoras_EditoraIdEditora",
                table: "Livros",
                column: "EditoraIdEditora",
                principalTable: "Editoras",
                principalColumn: "id_editora");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Editoras_id_editora",
                table: "Livros",
                column: "id_editora",
                principalTable: "Editoras",
                principalColumn: "id_editora",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carteiras_Clientes_ClienteIdCliente",
                table: "Carteiras");

            migrationBuilder.DropForeignKey(
                name: "FK_Carteiras_Editoras_EditoraIdEditora",
                table: "Carteiras");

            migrationBuilder.DropForeignKey(
                name: "FK_Carteiras_Editoras_id_editora",
                table: "Carteiras");

            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Editoras_EditoraIdEditora",
                table: "Livros");

            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Editoras_id_editora",
                table: "Livros");

            migrationBuilder.DropTable(
                name: "Editoras");

            migrationBuilder.DropIndex(
                name: "IX_Livros_EditoraIdEditora",
                table: "Livros");

            migrationBuilder.DropIndex(
                name: "IX_Livros_id_editora",
                table: "Livros");

            migrationBuilder.DropIndex(
                name: "IX_Carteiras_EditoraIdEditora",
                table: "Carteiras");

            migrationBuilder.DropIndex(
                name: "IX_Carteiras_id_cliente",
                table: "Carteiras");

            migrationBuilder.DropIndex(
                name: "IX_Carteiras_id_editora",
                table: "Carteiras");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Carteira_Dono",
                table: "Carteiras");

            migrationBuilder.DropColumn(
                name: "EditoraIdEditora",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "id_editora",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "is_emprestavel",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "EditoraIdEditora",
                table: "Carteiras");

            migrationBuilder.DropColumn(
                name: "id_editora",
                table: "Carteiras");

            migrationBuilder.AlterColumn<int>(
                name: "id_cliente",
                table: "Carteiras",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClienteIdCliente",
                table: "Carteiras",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carteiras_id_cliente",
                table: "Carteiras",
                column: "id_cliente",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Carteiras_Clientes_ClienteIdCliente",
                table: "Carteiras",
                column: "ClienteIdCliente",
                principalTable: "Clientes",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
