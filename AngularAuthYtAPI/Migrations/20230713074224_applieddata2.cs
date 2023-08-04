using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularAuthYtAPI.Migrations
{
    public partial class applieddata2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PostedDate",
                table: "appliedjobstable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "appliedjobstable",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Qualification",
                table: "appliedjobstable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Salary",
                table: "appliedjobstable",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Vacancy",
                table: "appliedjobstable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "appliedjobstable");

            migrationBuilder.DropColumn(
                name: "Qualification",
                table: "appliedjobstable");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "appliedjobstable");

            migrationBuilder.DropColumn(
                name: "Vacancy",
                table: "appliedjobstable");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PostedDate",
                table: "appliedjobstable",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
