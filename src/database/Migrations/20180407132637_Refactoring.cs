using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace database.Migrations
{
    public partial class Refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Attachments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PostId",
                table: "Attachments",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Posts_PostId",
                table: "Attachments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Posts_PostId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_PostId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Attachments");
        }
    }
}
