using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace database.Migrations
{
    public partial class LessonChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HomeWorkDescription",
                table: "Lessons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LessonDescription",
                table: "Lessons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeWorkDescription",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonDescription",
                table: "Lessons");
        }
    }
}
