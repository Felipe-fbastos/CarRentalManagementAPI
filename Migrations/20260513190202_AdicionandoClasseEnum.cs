using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoClasseEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeUser",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeUser",
                table: "Clients");
        }
    }
}
