using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elections.Migrations
{
    public partial class CleanUpStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Archived",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Seconded",
                table: "Candidates");

            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ignored",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ready",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Ignored",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "Ready",
                table: "Candidates");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Candidates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Archived",
                table: "Candidates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Seconded",
                table: "Candidates",
                nullable: false,
                defaultValue: false);
        }
    }
}
