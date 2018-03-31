using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace database.Migrations
{
    public partial class CourseChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Lessons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Courses_CourseId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courses");
        }
    }
}
