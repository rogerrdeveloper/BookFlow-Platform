using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprestimoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class COrrigir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LivroEdicao",
                table: "Livros",
                newName: "livro_edicao");

            migrationBuilder.AlterColumn<string>(
                name: "livro_edicao",
                table: "Livros",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "livro_edicao",
                table: "Livros",
                newName: "LivroEdicao");

            migrationBuilder.AlterColumn<string>(
                name: "LivroEdicao",
                table: "Livros",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
