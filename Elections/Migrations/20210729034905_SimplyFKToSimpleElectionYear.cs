using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elections.Migrations
{
    public partial class SimplyFKToSimpleElectionYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Elections_ElectionId",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_ElectionId",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ElectionId",
                table: "Candidates");

            migrationBuilder.AddColumn<int>(
                name: "ElectionYear",
                table: "Candidates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElectionYear",
                table: "Candidates");

            migrationBuilder.AddColumn<int>(
                name: "ElectionId",
                table: "Candidates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ElectionId",
                table: "Candidates",
                column: "ElectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidates_Elections_ElectionId",
                table: "Candidates",
                column: "ElectionId",
                principalTable: "Elections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
