using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace database.Migrations
{
    public partial class BirthYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BirthYear",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthYear",
                table: "AspNetUsers");
        }
    }
}
