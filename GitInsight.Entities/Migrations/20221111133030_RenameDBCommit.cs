using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitInsight.Entities.Migrations
{
    /// <inheritdoc />
    public partial class RenameDBAnalysis_s : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DBFrequency_DBAnalysis_s_DBAnalysis_sId",
                table: "DBFrequency");

            migrationBuilder.DropTable(
                name: "DBAnalysis_s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DBFrequency",
                table: "DBFrequency");

            migrationBuilder.DropColumn(
                name: "DBAnalysis_sId",
                table: "DBFrequency");

            migrationBuilder.RenameTable(
                name: "DBFrequency",
                newName: "DBFrequencies");

            migrationBuilder.AlterColumn<int>(
                name: "Frequency",
                table: "DBFrequencies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "DBFrequencies",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "DBAnalysisId",
                table: "DBFrequencies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DBFrequencies",
                table: "DBFrequencies",
                columns: new[] { "DBAnalysisId", "Date" });

            migrationBuilder.CreateTable(
                name: "DBAnalysis_s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LatestCommitId = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GitRepository = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBAnalysis_s", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DBFrequencies_DBAnalysis_s_DBAnalysisId",
                table: "DBFrequencies",
                column: "DBAnalysisId",
                principalTable: "DBAnalysis_s",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DBFrequencies_DBAnalysis_s_DBAnalysisId",
                table: "DBFrequencies");

            migrationBuilder.DropTable(
                name: "DBAnalysis_s");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DBFrequencies",
                table: "DBFrequencies");

            migrationBuilder.DropColumn(
                name: "DBAnalysisId",
                table: "DBFrequencies");

            migrationBuilder.RenameTable(
                name: "DBFrequencies",
                newName: "DBFrequency");

            migrationBuilder.AlterColumn<int>(
                name: "Frequency",
                table: "DBFrequency",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "DBFrequency",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "DBAnalysis_sId",
                table: "DBFrequency",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DBFrequency",
                table: "DBFrequency",
                columns: new[] { "DBAnalysis_sId", "Date" });

            migrationBuilder.CreateTable(
                name: "DBAnalysis_s",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    CommitID = table.Column<int>(type: "INTEGER", nullable: false),
                    GitRepository = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBAnalysis_s", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DBFrequency_DBAnalysis_s_DBAnalysis_sId",
                table: "DBFrequency",
                column: "DBAnalysis_sId",
                principalTable: "DBAnalysis_s",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
