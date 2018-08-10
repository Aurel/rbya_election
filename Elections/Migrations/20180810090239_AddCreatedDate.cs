using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Elections.Migrations
{
    public partial class AddCreatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Submitter",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldDefaultValue: "elections@rbya.org");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Candidates",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Candidates");

            migrationBuilder.AlterColumn<string>(
                name: "Submitter",
                table: "Candidates",
                nullable: false,
                defaultValue: "elections@rbya.org",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
