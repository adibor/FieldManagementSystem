using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FieldManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAreaFieldNameAndRemoveAreaUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaUnit",
                table: "Fields");

            migrationBuilder.RenameColumn(
                name: "Area",
                table: "Fields",
                newName: "AreaHectares");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AreaHectares",
                table: "Fields",
                newName: "Area");

            migrationBuilder.AddColumn<string>(
                name: "AreaUnit",
                table: "Fields",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
