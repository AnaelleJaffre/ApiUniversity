using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiUniversity.Migrations
{
    /// <inheritdoc />
    public partial class DatabasePart2_correction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Courses",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                newName: "IX_Courses_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Courses",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                newName: "IX_Courses_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }
    }
}
