using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "students");

            migrationBuilder.CreateTable(
                name: "StudentSnapshots",
                schema: "students",
                columns: table => new
                {
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Aggregate = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSnapshots", x => new { x.Version, x.AggregateId });
                });

            migrationBuilder.CreateTable(
                name: "StudentStoreEvents",
                schema: "students",
                columns: table => new
                {
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    EventType = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Event = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStoreEvents", x => new { x.Version, x.AggregateId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentSnapshots",
                schema: "students");

            migrationBuilder.DropTable(
                name: "StudentStoreEvents",
                schema: "students");
        }
    }
}
