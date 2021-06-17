using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace E_Invoice_API.Data.Migrations
{
    public partial class AddMailHistoryTableDeleteMailSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailSetting");

            migrationBuilder.CreateTable(
                name: "MailHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MailSendToEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendMailDateTime = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    EmailTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailHistory_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailHistory_UserId",
                table: "MailHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailHistory");

            migrationBuilder.CreateTable(
                name: "MailSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EncryptedPassword = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Host = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSetting", x => x.Id);
                });
        }
    }
}
