using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmprestimoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class BookFlowCompleto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "lce_multaDescontada",
                table: "LIVRO_CLIENTE_EMPRESTIMO",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "situacao_conta",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldDefaultValue: "A");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lce_multaDescontada",
                table: "LIVRO_CLIENTE_EMPRESTIMO");

            migrationBuilder.AlterColumn<string>(
                name: "situacao_conta",
                table: "Clientes",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                defaultValue: "A",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
