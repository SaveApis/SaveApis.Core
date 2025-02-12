using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveApis.Core.Common.Domains.Logging.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class CreateLoggingTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Logging");

            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Entries",
                schema: "Logging",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LoggedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LogType = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AffectedEntityId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AffectedEntityName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    State = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LoggedBy = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Values",
                schema: "Logging",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AttributeName = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OldValue = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NewValue = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogEntryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Values_Entries_LogEntryId",
                        column: x => x.LogEntryId,
                        principalSchema: "Logging",
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_AffectedEntityId",
                schema: "Logging",
                table: "Entries",
                column: "AffectedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_LogType",
                schema: "Logging",
                table: "Entries",
                column: "LogType");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_State",
                schema: "Logging",
                table: "Entries",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Values_LogEntryId_AttributeName",
                schema: "Logging",
                table: "Values",
                columns: new[] { "LogEntryId", "AttributeName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Values",
                schema: "Logging");

            migrationBuilder.DropTable(
                name: "Entries",
                schema: "Logging");
        }
    }
}
