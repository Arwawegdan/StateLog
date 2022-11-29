using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace StateLog.Server.Migrations
{
    /// <inheritdoc />
    public partial class removingtagvaluedatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagValueDataType",
                table: "StateLogCustomTags");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TagValueDataType",
                table: "StateLogCustomTags",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
