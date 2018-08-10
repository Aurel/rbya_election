using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elections.Migrations
{
    public partial class AddStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Candidates");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Seconded",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Seconded",
                table: "Candidates");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Candidates",
                nullable: true);
        }
    }
}
