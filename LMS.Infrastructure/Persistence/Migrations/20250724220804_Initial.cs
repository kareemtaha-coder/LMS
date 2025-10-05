using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Curriculums_CurriculumId",
                table: "Chapters");

            migrationBuilder.DropForeignKey(
                name: "FK_ExampleItems_LessonContents_ExamplesGridContentId",
                table: "ExampleItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonContents_Lessons_LessonId",
                table: "LessonContents");

            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Chapters_ChapterId",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExampleItems",
                table: "ExampleItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters");

            migrationBuilder.RenameTable(
                name: "Lessons",
                newName: "Lesson");

            migrationBuilder.RenameTable(
                name: "ExampleItems",
                newName: "ExampleItem");

            migrationBuilder.RenameTable(
                name: "Chapters",
                newName: "Chapter");

            migrationBuilder.RenameIndex(
                name: "IX_Lessons_ChapterId",
                table: "Lesson",
                newName: "IX_Lesson_ChapterId");

            migrationBuilder.RenameIndex(
                name: "IX_ExampleItems_ExamplesGridContentId",
                table: "ExampleItem",
                newName: "IX_ExampleItem_ExamplesGridContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Chapters_CurriculumId",
                table: "Chapter",
                newName: "IX_Chapter_CurriculumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lesson",
                table: "Lesson",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExampleItem",
                table: "ExampleItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapter",
                table: "Chapter",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_Curriculums_CurriculumId",
                table: "Chapter",
                column: "CurriculumId",
                principalTable: "Curriculums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExampleItem_LessonContents_ExamplesGridContentId",
                table: "ExampleItem",
                column: "ExamplesGridContentId",
                principalTable: "LessonContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lesson_Chapter_ChapterId",
                table: "Lesson",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonContents_Lesson_LessonId",
                table: "LessonContents",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_Curriculums_CurriculumId",
                table: "Chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_ExampleItem_LessonContents_ExamplesGridContentId",
                table: "ExampleItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Lesson_Chapter_ChapterId",
                table: "Lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonContents_Lesson_LessonId",
                table: "LessonContents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lesson",
                table: "Lesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExampleItem",
                table: "ExampleItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapter",
                table: "Chapter");

            migrationBuilder.RenameTable(
                name: "Lesson",
                newName: "Lessons");

            migrationBuilder.RenameTable(
                name: "ExampleItem",
                newName: "ExampleItems");

            migrationBuilder.RenameTable(
                name: "Chapter",
                newName: "Chapters");

            migrationBuilder.RenameIndex(
                name: "IX_Lesson_ChapterId",
                table: "Lessons",
                newName: "IX_Lessons_ChapterId");

            migrationBuilder.RenameIndex(
                name: "IX_ExampleItem_ExamplesGridContentId",
                table: "ExampleItems",
                newName: "IX_ExampleItems_ExamplesGridContentId");

            migrationBuilder.RenameIndex(
                name: "IX_Chapter_CurriculumId",
                table: "Chapters",
                newName: "IX_Chapters_CurriculumId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lessons",
                table: "Lessons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExampleItems",
                table: "ExampleItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapters",
                table: "Chapters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Curriculums_CurriculumId",
                table: "Chapters",
                column: "CurriculumId",
                principalTable: "Curriculums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExampleItems_LessonContents_ExamplesGridContentId",
                table: "ExampleItems",
                column: "ExamplesGridContentId",
                principalTable: "LessonContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonContents_Lessons_LessonId",
                table: "LessonContents",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Chapters_ChapterId",
                table: "Lessons",
                column: "ChapterId",
                principalTable: "Chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
