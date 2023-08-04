using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularAuthYtAPI.Migrations
{
    public partial class resumeupload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_resume",
                table: "resume");

            migrationBuilder.RenameTable(
                name: "resume",
                newName: "ResumesUpload");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                table: "ResumesUpload",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ResumesUpload",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "ResumesUpload",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResumesUpload",
                table: "ResumesUpload",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResumesUpload_JobId",
                table: "ResumesUpload",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResumesUpload_jobs_JobId",
                table: "ResumesUpload",
                column: "JobId",
                principalTable: "jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResumesUpload_jobs_JobId",
                table: "ResumesUpload");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResumesUpload",
                table: "ResumesUpload");

            migrationBuilder.DropIndex(
                name: "IX_ResumesUpload_JobId",
                table: "ResumesUpload");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "ResumesUpload");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ResumesUpload");

            migrationBuilder.DropColumn(
                name: "email",
                table: "ResumesUpload");

            migrationBuilder.RenameTable(
                name: "ResumesUpload",
                newName: "resume");

            migrationBuilder.AddPrimaryKey(
                name: "PK_resume",
                table: "resume",
                column: "ResumeId");
        }
    }
}
