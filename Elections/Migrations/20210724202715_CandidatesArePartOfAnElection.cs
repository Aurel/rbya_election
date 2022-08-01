using Microsoft.EntityFrameworkCore.Migrations;

namespace Elections.Migrations
{
	public partial class CandidatesArePartOfAnElection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElectionYear",
                table: "Candidates",
                type: "int",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Candidates_Elections_ElectionYear",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_ElectionYear",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "ElectionYear",
                table: "Candidates");
        }
    }
}
