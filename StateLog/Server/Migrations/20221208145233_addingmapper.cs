using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StateLog.Server.Migrations
{
    /// <inheritdoc />
    public partial class addingmapper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mapper",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SchemaName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedColoumn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedColumnType = table.Column<int>(type: "int", nullable: false),
                    ChangedColumnNewValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mapper", x => new { x.Id, x.DateTime });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mapper");
        }
    }
}
