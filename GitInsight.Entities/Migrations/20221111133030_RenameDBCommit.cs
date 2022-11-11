using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitInsight.Entities.Migrations
{
    /// <inheritdoc />
    public partial class RenameDBCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DBFrequency_DBCommit_DBCommitId",
                table: "DBFrequency");

            migrationBuilder.DropTable(
                name: "DBCommit");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DBFrequency",
                table: "DBFrequency");

            migrationBuilder.DropColumn(
                name: "DBCommitId",
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
                name: "DBAnalyses",
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
                    table.PrimaryKey("PK_DBAnalyses", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DBFrequencies_DBAnalyses_DBAnalysisId",
                table: "DBFrequencies",
                column: "DBAnalysisId",
                principalTable: "DBAnalyses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DBFrequencies_DBAnalyses_DBAnalysisId",
                table: "DBFrequencies");

            migrationBuilder.DropTable(
                name: "DBAnalyses");

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
                name: "DBCommitId",
                table: "DBFrequency",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DBFrequency",
                table: "DBFrequency",
                columns: new[] { "DBCommitId", "Date" });

            migrationBuilder.CreateTable(
                name: "DBCommit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    CommitID = table.Column<int>(type: "INTEGER", nullable: false),
                    GitRepository = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBCommit", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DBFrequency_DBCommit_DBCommitId",
                table: "DBFrequency",
                column: "DBCommitId",
                principalTable: "DBCommit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
