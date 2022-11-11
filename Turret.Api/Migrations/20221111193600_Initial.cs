using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Turret.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    hashedpassword = table.Column<byte[]>(name: "hashed_password", type: "bytea", nullable: false),
                    salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    displayname = table.Column<string>(name: "display_name", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                    table.CheckConstraint("CK_user_display_name_Length", "LENGTH(\"display_name\") <= 64");
                    table.CheckConstraint("CK_user_email_Length", "LENGTH(\"email\") <= 320");
                    table.CheckConstraint("CK_user_email_Regex", "\"email\" ~ '^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$'");
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_email",
                table: "user",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
