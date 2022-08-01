using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elections.Migrations
{
    public partial class RemoveElections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Elections_ElectionYear",
                table: "Candidates");

            migrationBuilder.DropTable(
                name: "Elections");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_ElectionYear",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ElectionYear",
                table: "Candidates");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElectionYear",
                table: "Candidates",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    Year = table.Column<int>(nullable: false),
                    ElectionDay = table.Column<DateTime>(nullable: false),
                    NominationCutoff = table.Column<DateTime>(nullable: false),
                    NominationsOpen = table.Column<bool>(nullable: false),
                    VotingOpen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.Year);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ElectionYear",
                table: "Candidates",
                column: "ElectionYear");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Elections_ElectionYear",
                table: "Candidates",
                column: "ElectionYear",
                principalTable: "Elections",
                principalColumn: "Year",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
