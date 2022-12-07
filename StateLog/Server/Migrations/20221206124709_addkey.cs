using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StateLog.Server.Migrations
{
    /// <inheritdoc />
    public partial class addkey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Nationalities_NationalityId_NationalityProductId_NationalityBranchId_NationalityCreatorId_NationalityName_National~",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_NationalityId_NationalityProductId_NationalityBranchId_NationalityCreatorId_NationalityName_NationalityTagValue_Na~",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalityBranchId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalityCreatorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalityName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalityPartitionKey",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalityProductId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalityTagName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NationalityTagValue",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NationalityBranchId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NationalityCreatorId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalityName",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalityPartitionKey",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "NationalityProductId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalityTagName",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalityTagValue",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_NationalityId_NationalityProductId_NationalityBranchId_NationalityCreatorId_NationalityName_NationalityTagValue_Na~",
                table: "Employees",
                columns: new[] { "NationalityId", "NationalityProductId", "NationalityBranchId", "NationalityCreatorId", "NationalityName", "NationalityTagValue", "NationalityTagName", "NationalityPartitionKey" });

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Nationalities_NationalityId_NationalityProductId_NationalityBranchId_NationalityCreatorId_NationalityName_National~",
                table: "Employees",
                columns: new[] { "NationalityId", "NationalityProductId", "NationalityBranchId", "NationalityCreatorId", "NationalityName", "NationalityTagValue", "NationalityTagName", "NationalityPartitionKey" },
                principalTable: "Nationalities",
                principalColumns: new[] { "Id", "ProductId", "BranchId", "CreatorId", "Name", "TagValue", "TagName", "PartitionKey" });
        }
    }
}
