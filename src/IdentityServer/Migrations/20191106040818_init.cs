﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    RoleName = table.Column<string>(nullable: true),
                    _rights = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    RoleId = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    ProviderName = table.Column<string>(nullable: true),
                    ProviderSubjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName", "_rights" },
                values: new object[] { new Guid("be599f58-c890-4cc5-af31-afd6f3940a2d"), "Admin", " " });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName", "_rights" },
                values: new object[] { new Guid("b30d5e34-4d2e-4246-bfd6-18c3f8bbce7d"), "Manager", " " });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "RoleName", "_rights" },
                values: new object[] { new Guid("5a9ea9a8-9053-4a7b-8a79-e1c5cb8743a8"), "User", " " });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Name", "PasswordHash", "ProviderName", "ProviderSubjectId", "RoleId", "Salt" },
                values: new object[] { new Guid("9eef3f97-4634-4767-98a5-0d91883d1723"), "alice@alice.com", "Alice", "+CddwLmdTnoB5t7oKc49IIL9fNj8cphZajO451sEJ5c=", null, null, new Guid("be599f58-c890-4cc5-af31-afd6f3940a2d"), "salt1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Name", "PasswordHash", "ProviderName", "ProviderSubjectId", "RoleId", "Salt" },
                values: new object[] { new Guid("71a5ad45-4850-4889-8694-675bf3357e16"), "bob@bob.com", "Bob", "6dq9jIcKvABLyU1bT1y6ChK2U/hKx4fH+eDgVeTZ7/U=", null, null, new Guid("b30d5e34-4d2e-4246-bfd6-18c3f8bbce7d"), "salt2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "Name", "PasswordHash", "ProviderName", "ProviderSubjectId", "RoleId", "Salt" },
                values: new object[] { new Guid("08f14f11-4929-4089-9c10-d2be044da750"), "eve@eve.com", "Eve", "hBh3WnVkOa+khsTF8YPoD54KbbuibzWLB8XKB9ci788=", null, null, new Guid("5a9ea9a8-9053-4a7b-8a79-e1c5cb8743a8"), "salt3" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
