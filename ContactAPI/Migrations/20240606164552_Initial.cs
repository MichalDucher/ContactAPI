using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContactAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    categoryid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoryname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.categoryid);
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    contactid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    categoryid = table.Column<int>(type: "integer", nullable: false),
                    subcategory = table.Column<string>(type: "text", nullable: false),
                    phonenumber = table.Column<string>(type: "text", nullable: false),
                    birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacts", x => x.contactid);
                    table.ForeignKey(
                        name: "FK_contacts_categories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "categories",
                        principalColumn: "categoryid",
                        onDelete: ReferentialAction.Cascade); // Dodanie klucza obcego
                });

            migrationBuilder.CreateTable(
                name: "subcategories",
                columns: table => new
                {
                    subcategoryid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subcategoryname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subcategories", x => x.subcategoryid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                });

            // Wstawianie przykładowych danych
            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "categoryname" },
                values: new object[,]
                {
                    { "służbowy" },
                    { "prywatny" },
                    { "inny" }
                });

            migrationBuilder.InsertData(
                table: "subcategories",
                columns: new[] { "subcategoryname" },
                values: new object[,]
                {
                    { "szef" },
                    { "pracownik" },
                    { "klient" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "username", "password" },
                values: new object[,]
                {
                    { "admin", HashPassword("password") },
                });

            migrationBuilder.InsertData(
                table: "contacts",
                columns: new[] { "firstname", "lastname", "email", "password", "categoryid", "subcategory", "phonenumber", "birthdate" },
                values: new object[,]
                {
                    { "Marek", "Nowolipiński", "marian@wp.com", "password", 1, "szef", "123-456-7890", DateTime.UtcNow.AddYears(-30) },
                    { "Janek", "Krótki", "niski@gmail.com", "password", 3, "inny", "098-765-4321", DateTime.UtcNow.AddYears(-25) }
                });
        }

        private static string HashPassword(string password)
        {
            // Tworzenie instancji obiektu SHA256
            using (var sha256 = SHA256.Create())
            {
                // Obliczanie hasha dla hasła
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Konwersja bajtów na ciąg znaków w formacie szesnastkowym
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                // Zwracanie zahashowanego hasła
                return builder.ToString();
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "subcategories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
