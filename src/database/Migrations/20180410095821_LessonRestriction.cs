using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace database.Migrations
{
    public partial class LessonRestriction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Lessons",
                newName: "LessonStartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "HomeWorkEnd",
                table: "Lessons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LessonEndDate",
                table: "Lessons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeWorkEnd",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "LessonEndDate",
                table: "Lessons");

            migrationBuilder.RenameColumn(
                name: "LessonStartDate",
                table: "Lessons",
                newName: "Date");
        }
    }
}
