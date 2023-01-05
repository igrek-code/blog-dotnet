using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bloogs.Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47e9f11c-01e7-4e94-aa88-84ae3ad53df5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76996480-dc14-414b-b789-0032d6ac07a7");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "4c98ab8c-8649-4024-95ac-d232078635d1", "fc5cf58f-ac8f-4219-ab0a-a308b8361230" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c98ab8c-8649-4024-95ac-d232078635d1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fc5cf58f-ac8f-4219-ab0a-a308b8361230");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blog_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blogId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PosterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_AspNetUsers_PosterId",
                        column: x => x.PosterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Post_Blog_blogId",
                        column: x => x.blogId,
                        principalTable: "Blog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3f88b776-2ae0-4827-8d71-efb5a810c961", "015c7044-ae49-406c-bf8c-378dcb29c39e", "Supervisor", "SUPERVISOR" },
                    { "df128d62-20bf-458b-95c2-4316eee609c0", "836f7098-6d05-42d1-8f3a-3cd8f96718e1", "Admin", "ADMIN" },
                    { "ee7d5d60-1b6b-4e91-a407-ea1284615760", "0550ac9a-9c99-4501-810e-22bdacf69922", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "f30cf4a6-ee31-4823-b9d2-1df60a48055e", 0, "434d4b7c-8617-4036-a3a3-2e40ffbd488e", "supervisor@blog.fr", true, "Supervisor", "Supervisor", false, null, "SUPERVISOR@BLOG.FR", "SUPERVISOR", "AQAAAAEAACcQAAAAELNpHWtxpX4MSWPqMbMhUqTvlc8tkd0TlqMhhW3JcVL/QIXUCMsnRy43dBohP+x8pw==", null, false, null, "82450ccf-4a35-4d61-a0c2-61e2ce866972", false, "supervisor" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "3f88b776-2ae0-4827-8d71-efb5a810c961", "f30cf4a6-ee31-4823-b9d2-1df60a48055e" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PostId",
                table: "AspNetUsers",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_OwnerId",
                table: "Blog",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_OwnerId",
                table: "Comment",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_blogId",
                table: "Post",
                column: "blogId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_PosterId",
                table: "Post",
                column: "PosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Post_PostId",
                table: "AspNetUsers",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Post_PostId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PostId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df128d62-20bf-458b-95c2-4316eee609c0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee7d5d60-1b6b-4e91-a407-ea1284615760");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3f88b776-2ae0-4827-8d71-efb5a810c961", "f30cf4a6-ee31-4823-b9d2-1df60a48055e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f88b776-2ae0-4827-8d71-efb5a810c961");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "f30cf4a6-ee31-4823-b9d2-1df60a48055e");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "47e9f11c-01e7-4e94-aa88-84ae3ad53df5", "e6e0ca82-7077-4d02-929b-c708c8ee6890", "Admin", "ADMIN" },
                    { "4c98ab8c-8649-4024-95ac-d232078635d1", "352ecf84-aaf8-4d19-bc7d-987a41e74ae3", "Supervisor", "SUPERVISOR" },
                    { "76996480-dc14-414b-b789-0032d6ac07a7", "ce702eae-39f6-4ed4-a4e8-350c2071e4a1", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "fc5cf58f-ac8f-4219-ab0a-a308b8361230", 0, "2edb47b0-5e0d-46bd-ae15-ccc7b5575ea6", "supervisor@blog.fr", true, null, null, false, null, "SUPERVISOR@BLOG.FR", "SUPERVISOR", "AQAAAAEAACcQAAAAEC8g3lEz2aM3XqPtaFLY7Pndr9x/eiKks+WKpDHKNbwyg+Jkmlr5fusoSMBOsMauuA==", null, false, "ec659c44-2bb5-4c14-9047-254f990d2310", false, "supervisor" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "4c98ab8c-8649-4024-95ac-d232078635d1", "fc5cf58f-ac8f-4219-ab0a-a308b8361230" });
        }
    }
}
