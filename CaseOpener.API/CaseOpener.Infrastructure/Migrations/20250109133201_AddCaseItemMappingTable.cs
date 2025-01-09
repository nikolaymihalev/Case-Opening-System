using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseOpener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseItemMappingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Probability",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Items",
                table: "Cases");

            migrationBuilder.CreateTable(
                name: "CaseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Case item identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false, comment: "Case's identifier"),
                    ItemId = table.Column<int>(type: "int", nullable: false, comment: "Item's identifier"),
                    Probability = table.Column<double>(type: "float", nullable: false, comment: "Item's probability chance")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseItems_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents mapping between case and item");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC5q0ixLjaEtD9hTkKd4r1dETz78h0Wx8kO6v7kalp2jEJ/hLN33m9X543qxNJPifQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a646737-b3ab-4595-9770-2c744e5808c6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP8EYJ4To9JlPaG0fPuKkdvaoL0Ie7Cmavuy4xqcijyxphgy0Y89ibPAIAhpXMUzdw==");

            migrationBuilder.CreateIndex(
                name: "IX_CaseItems_CaseId",
                table: "CaseItems",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseItems_ItemId",
                table: "CaseItems",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseItems");

            migrationBuilder.AddColumn<double>(
                name: "Probability",
                table: "Items",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                comment: "Item's probability chance");

            migrationBuilder.AddColumn<string>(
                name: "Items",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Case's items");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOldNi/GRoASPphHJclntAesvl0+a+TW8L+twQjFM/7cN5yoG1ZvCpGfOb2RInn/PA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a646737-b3ab-4595-9770-2c744e5808c6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEI/QqBkgbDDNbW/MgIHhtb3rlXPbe4WFNqAh49lNA4o6acWZZKQd8zMMMun7w7Ahfw==");
        }
    }
}
