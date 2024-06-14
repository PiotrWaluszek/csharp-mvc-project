using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryMan.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UzytkownikModel",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UzytkownikModel", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "WydawnictwoModel",
                columns: table => new
                {
                    PublisherName = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Founded = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WydawnictwoModel", x => x.PublisherName);
                });

            migrationBuilder.CreateTable(
                name: "KsiazkaModel",
                columns: table => new
                {
                    BookName = table.Column<string>(type: "TEXT", nullable: false),
                    PublisherName = table.Column<string>(type: "TEXT", nullable: false),
                    Genre = table.Column<string>(type: "TEXT", nullable: false),
                    AverageRating = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KsiazkaModel", x => x.BookName);
                    table.ForeignKey(
                        name: "FK_KsiazkaModel_WydawnictwoModel_PublisherName",
                        column: x => x.PublisherName,
                        principalTable: "WydawnictwoModel",
                        principalColumn: "PublisherName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecenzjaModel",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    BookName = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecenzjaModel", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_RecenzjaModel_KsiazkaModel_BookName",
                        column: x => x.BookName,
                        principalTable: "KsiazkaModel",
                        principalColumn: "BookName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecenzjaModel_UzytkownikModel_UserID",
                        column: x => x.UserID,
                        principalTable: "UzytkownikModel",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KsiazkaModel_PublisherName",
                table: "KsiazkaModel",
                column: "PublisherName");

            migrationBuilder.CreateIndex(
                name: "IX_RecenzjaModel_BookName",
                table: "RecenzjaModel",
                column: "BookName");

            migrationBuilder.CreateIndex(
                name: "IX_RecenzjaModel_UserID",
                table: "RecenzjaModel",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecenzjaModel");

            migrationBuilder.DropTable(
                name: "KsiazkaModel");

            migrationBuilder.DropTable(
                name: "UzytkownikModel");

            migrationBuilder.DropTable(
                name: "WydawnictwoModel");
        }
    }
}
