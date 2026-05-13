using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class CorrigindoPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Passoword",
                table: "Clients",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Clients",
                newName: "Passoword");
        }
    }
}
