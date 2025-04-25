using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject.Migrations
{
    /// <inheritdoc />
    public partial class ChagingOnDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_QuestionOptions_OptionId",
                table: "AnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateStats_Templates_TemplateId",
                table: "TemplateStats");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateTags_Tags_TagId",
                table: "TemplateTags");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_QuestionOptions_OptionId",
                table: "AnswerOptions",
                column: "OptionId",
                principalTable: "QuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateStats_Templates_TemplateId",
                table: "TemplateStats",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateTags_Tags_TagId",
                table: "TemplateTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_QuestionOptions_OptionId",
                table: "AnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateStats_Templates_TemplateId",
                table: "TemplateStats");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateTags_Tags_TagId",
                table: "TemplateTags");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_QuestionOptions_OptionId",
                table: "AnswerOptions",
                column: "OptionId",
                principalTable: "QuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateStats_Templates_TemplateId",
                table: "TemplateStats",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateTags_Tags_TagId",
                table: "TemplateTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
