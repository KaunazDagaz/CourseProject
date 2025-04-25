using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject.Migrations
{
    /// <inheritdoc />
    public partial class FixingAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_QuestionOptions_OptionId1",
                table: "AnswerOptions");

            migrationBuilder.DropIndex(
                name: "IX_AnswerOptions_OptionId1",
                table: "AnswerOptions");

            migrationBuilder.DropColumn(
                name: "OptionId1",
                table: "AnswerOptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OptionId1",
                table: "AnswerOptions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_OptionId1",
                table: "AnswerOptions",
                column: "OptionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_QuestionOptions_OptionId1",
                table: "AnswerOptions",
                column: "OptionId1",
                principalTable: "QuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
