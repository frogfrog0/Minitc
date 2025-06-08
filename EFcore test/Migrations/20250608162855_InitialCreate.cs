using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFcore_test.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kursy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kursy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Studenci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IdStudenta = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KursStudent",
                columns: table => new
                {
                    KursyId = table.Column<int>(type: "INTEGER", nullable: false),
                    StudenciId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KursStudent", x => new { x.KursyId, x.StudenciId });
                    table.ForeignKey(
                        name: "FK_KursStudent_Kursy_KursyId",
                        column: x => x.KursyId,
                        principalTable: "Kursy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KursStudent_Studenci_StudenciId",
                        column: x => x.StudenciId,
                        principalTable: "Studenci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Imie = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DataUrodzenia = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RokStudiow = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profile_Studenci_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kursy",
                columns: new[] { "Id", "Nazwa" },
                values: new object[,]
                {
                    { 1, "MD" },
                    { 2, "ZSI" },
                    { 3, "SSI" }
                });

            migrationBuilder.InsertData(
                table: "Studenci",
                columns: new[] { "Id", "IdStudenta" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 }
                });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "Id", "DataUrodzenia", "Imie", "Nazwisko", "RokStudiow", "StudentId" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jan", "Kowalski", 2, 1 },
                    { 2, new DateTime(2001, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anna", "Nowak", 1, 2 },
                    { 3, new DateTime(1999, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Piotr", "Zieliński", 3, 3 },
                    { 4, new DateTime(2000, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maria", "Wiśniewska", 2, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_KursStudent_StudenciId",
                table: "KursStudent",
                column: "StudenciId");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_StudentId",
                table: "Profile",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Studenci_IdStudenta",
                table: "Studenci",
                column: "IdStudenta",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KursStudent");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "Kursy");

            migrationBuilder.DropTable(
                name: "Studenci");
        }
    }
}
