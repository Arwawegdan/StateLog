using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StateLog.Server.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NationalityReducer",
                table: "NationalityReducer");

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "NationalityReducer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "TagValue",
                table: "NationalityReducer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NationalityReducer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NationalityReducer",
                table: "NationalityReducer",
                columns: new[] { "Id", "Datetime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NationalityReducer",
                table: "NationalityReducer");

            migrationBuilder.AlterColumn<string>(
                name: "TagValue",
                table: "NationalityReducer",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TagName",
                table: "NationalityReducer",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NationalityReducer",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NationalityReducer",
                table: "NationalityReducer",
                columns: new[] { "Id", "ProductId", "BranchId", "CreatorId", "Name", "TagValue", "TagName", "Datetime" });
        }
    }
}
