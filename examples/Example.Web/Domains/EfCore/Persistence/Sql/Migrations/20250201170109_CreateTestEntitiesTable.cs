using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example.Web.Domains.EfCore.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class CreateTestEntitiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "EfCore");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TestEntities",
                schema: "EfCore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestEntities", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                schema: "EfCore",
                table: "TestEntities",
                columns: new[] { "Id", "CreatedAt", "FirstName", "LastName", "UpdatedAt" },
                values: new object[] { new Guid("f5daebb9-a4b3-4e75-81ab-6607c770316a"), new DateTime(2025, 2, 1, 17, 1, 8, 685, DateTimeKind.Utc).AddTicks(6061), "Test", "Test", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestEntities",
                schema: "EfCore");
        }
    }
}
