using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StateLog.Server.Migrations
{
    /// <inheritdoc />
    public partial class addemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfEmployees",
                table: "Nationalities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PartitionKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TagName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TagValue = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NationalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalityProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NationalityBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NationalityCreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NationalityName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NationalityTagValue = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NationalityTagName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NationalityPartitionKey = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => new { x.Id, x.Name, x.TagValue, x.TagName, x.PartitionKey, x.NationalityId });
                    table.ForeignKey(
                        name: "FK_Employees_Nationalities_NationalityId_NationalityProductId_NationalityBranchId_NationalityCreatorId_NationalityName_National~",
                        columns: x => new { x.NationalityId, x.NationalityProductId, x.NationalityBranchId, x.NationalityCreatorId, x.NationalityName, x.NationalityTagValue, x.NationalityTagName, x.NationalityPartitionKey },
                        principalTable: "Nationalities",
                        principalColumns: new[] { "Id", "ProductId", "BranchId", "CreatorId", "Name", "TagValue", "TagName", "PartitionKey" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_NationalityId_NationalityProductId_NationalityBranchId_NationalityCreatorId_NationalityName_NationalityTagValue_Na~",
                table: "Employees",
                columns: new[] { "NationalityId", "NationalityProductId", "NationalityBranchId", "NationalityCreatorId", "NationalityName", "NationalityTagValue", "NationalityTagName", "NationalityPartitionKey" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropColumn(
                name: "NoOfEmployees",
                table: "Nationalities");
        }
    }
}
