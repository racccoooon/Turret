using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turret.Api.Migrations
{
    /// <inheritdoc />
    public partial class Projects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    displayname = table.Column<string>(name: "display_name", type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_project", x => x.id);
                    // table.CheckConstraint("CK_project_description_Length", "(\"description\" IS NULL OR LENGTH(\"description\") <= 10000");
                    table.CheckConstraint("CK_project_display_name_Length", "LENGTH(\"display_name\") <= 64");
                    table.CheckConstraint("CK_project_key_Length", "LENGTH(\"key\") <= 8 AND LENGTH(\"key\") >= 3");
                });

            migrationBuilder.CreateIndex(
                name: "ix_project_key",
                table: "project",
                column: "key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project");
        }
    }
}
