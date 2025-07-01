using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI_based_Language_Teaching.Migrations
{
    /// <inheritdoc />
    public partial class returnItBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Curriculums",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Curricula",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Curricula_UserId",
                table: "Curricula",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Curricula_Users_UserId",
                table: "Curricula",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Curricula_Users_UserId",
                table: "Curricula");

            migrationBuilder.DropIndex(
                name: "IX_Curricula_UserId",
                table: "Curricula");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Curricula");

            migrationBuilder.AddColumn<string>(
                name: "Curriculums",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
