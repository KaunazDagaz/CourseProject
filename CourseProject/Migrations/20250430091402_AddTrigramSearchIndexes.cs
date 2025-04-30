using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseProject.Migrations
{
    /// <inheritdoc />
    public partial class AddTrigramSearchIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pg_trgm;");

            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_templates_title_trgm ON ""Templates"" USING gin (""Title"" gin_trgm_ops);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_templates_description_trgm ON ""Templates"" USING gin ((COALESCE(""Description"", '')) gin_trgm_ops);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_templates_topic_trgm ON ""Templates"" USING gin (""Topic"" gin_trgm_ops);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_questions_title_trgm ON ""Questions"" USING gin (""Title"" gin_trgm_ops);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_questions_description_trgm ON ""Questions"" USING gin ((COALESCE(""Description"", '')) gin_trgm_ops);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_question_options_text_trgm ON ""QuestionOptions"" USING gin (""Text"" gin_trgm_ops);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_comments_content_trgm ON ""Comments"" USING gin (""Content"" gin_trgm_ops);");
            migrationBuilder.Sql(@"CREATE INDEX IF NOT EXISTS ix_tags_name_trgm ON ""Tags"" USING gin (""Name"" gin_trgm_ops);");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Description",
                table: "Templates",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Title",
                table: "Templates",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Topic",
                table: "Templates",
                column: "Topic");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Description",
                table: "Questions",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Title",
                table: "Questions",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_Text",
                table: "QuestionOptions",
                column: "Text");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Content",
                table: "Comments",
                column: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_templates_title_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_templates_description_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_templates_topic_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_questions_title_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_questions_description_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_question_options_text_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_comments_content_trgm;");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ix_tags_name_trgm;");


            migrationBuilder.DropIndex(
                name: "IX_Templates_Description",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_Title",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Templates_Topic",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Name",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Questions_Description",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_Title",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionOptions_Text",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_Comments_Content",
                table: "Comments");
        }
    }
}
