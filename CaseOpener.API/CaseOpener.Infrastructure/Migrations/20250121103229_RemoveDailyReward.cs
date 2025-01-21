using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseOpener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDailyReward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyRewards");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKim1m29ABPp1LKBbpybMylggNdYhq3Iw8KbGQn7USvQ78a0xiLvbdSAP5MtXIFoFw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a646737-b3ab-4595-9770-2c744e5808c6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEKYIO332iZ7du41WEW7IoMgAOR96f/ch8u30RxbKyCKI/syNc/TIkYbbpbgKrk92+w==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Reward's identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false, comment: "Case's identifier"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User's identifier"),
                    LastClaimedDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Case's claimed date")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyRewards_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyRewards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents the user's daily reward");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJS5sS+TaW2My0N0wl4HGXzTp3fJw1LRa5KnTkpaKuGzpapyFsSNyqreSpjnSyUESw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a646737-b3ab-4595-9770-2c744e5808c6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEDihq00+bvOa5T/RIy+LLbUfeadQ5kO8ObfyjzVYgeu8ocvfqH88RpYX3e5r5I42Uw==");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRewards_CaseId",
                table: "DailyRewards",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRewards_UserId",
                table: "DailyRewards",
                column: "UserId");
        }
    }
}
