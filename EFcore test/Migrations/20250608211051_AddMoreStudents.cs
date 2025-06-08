using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFcore_test.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Studenci",
                columns: new[] { "Id", "IdStudenta" },
                values: new object[,]
                {
                    { 5, 5 },
                    { 6, 6 },
                    { 7, 7 },
                    { 8, 8 },
                    { 9, 9 },
                    { 10, 10 },
                    { 11, 11 },
                    { 12, 12 },
                    { 13, 13 },
                    { 14, 14 },
                    { 15, 15 },
                    { 16, 16 }
                });

            migrationBuilder.InsertData(
                table: "Profile",
                columns: new[] { "Id", "DataUrodzenia", "Imie", "Nazwisko", "RokStudiow", "StudentId" },
                values: new object[,]
                {
                    { 5, new DateTime(1998, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tomasz", "Lewandowski", 4, 5 },
                    { 6, new DateTime(2002, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Katarzyna", "Wójcik", 1, 6 },
                    { 7, new DateTime(2000, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Michał", "Kowalczyk", 2, 7 },
                    { 8, new DateTime(1999, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Magdalena", "Jankowska", 3, 8 },
                    { 9, new DateTime(2001, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Łukasz", "Mazur", 1, 9 },
                    { 10, new DateTime(1998, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agnieszka", "Krawczyk", 5, 10 },
                    { 11, new DateTime(2000, 6, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paweł", "Szymański", 2, 11 },
                    { 12, new DateTime(2002, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Joanna", "Dąbrowska", 1, 12 },
                    { 13, new DateTime(1999, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marcin", "Zając", 3, 13 },
                    { 14, new DateTime(2001, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Natalia", "Kozłowska", 2, 14 },
                    { 15, new DateTime(1998, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adam", "Jankowski", 4, 15 },
                    { 16, new DateTime(2000, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Aleksandra", "Wojciechowska", 2, 16 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Profile",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Studenci",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
