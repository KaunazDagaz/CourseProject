using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject.Migrations
{
    /// <inheritdoc />
    public partial class FixAnswerOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_OptionId",
                table: "AnswerOptions");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_OptionId",
                table: "AnswerOptions",
                column: "OptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_OptionId",
                table: "AnswerOptions");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_OptionId",
                table: "AnswerOptions",
                column: "OptionId",
                unique: true);
        }
    }
}
