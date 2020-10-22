using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MCLiveStatus.EntityFramework.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServerPingerSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AllowNotifyJoinable = table.Column<bool>(nullable: false),
                    AllowNotifyQueueJoinable = table.Column<bool>(nullable: false),
                    PingIntervalSeconds = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerPingerSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServerPingerSettings");
        }
    }
}
