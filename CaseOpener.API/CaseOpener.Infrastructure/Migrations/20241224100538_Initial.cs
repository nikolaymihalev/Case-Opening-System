using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseOpener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Case's identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Case's name"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false, comment: "Case's image"),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, comment: "Case's price"),
                    Items = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Case's items")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                },
                comment: "Represents the Case");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Item's indentifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false, comment: "Item's image"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Item's name"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Item's type"),
                    Rarity = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Item's rarity"),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, comment: "Item's amount"),
                    Probability = table.Column<double>(type: "float", nullable: false, comment: "Item's probability chance")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                },
                comment: "Represents the Item");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User's identifier"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "User's username"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "User's email"),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "User's password"),
                    Balance = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, comment: "User's balance"),
                    DateJoined = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date when user joined")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                },
                comment: "Represents the User");

            migrationBuilder.CreateTable(
                name: "CaseOpenings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Opening's identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User's identifier"),
                    CaseId = table.Column<int>(type: "int", nullable: false, comment: "Case's identifier"),
                    ItemId = table.Column<int>(type: "int", nullable: false, comment: "Item's identifier"),
                    DateOpened = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Opening's open date")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseOpenings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseOpenings_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseOpenings_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseOpenings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents case opening");

            migrationBuilder.CreateTable(
                name: "DailyRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Reward's identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User's identifier"),
                    CaseId = table.Column<int>(type: "int", nullable: false, comment: "Case's identifier"),
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

            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Inventory item's identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User's identifier"),
                    ItemId = table.Column<int>(type: "int", nullable: false, comment: "Item's identifier"),
                    AcquiredDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Inventory item's acquired date")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents the user's inventory item");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Transaction's identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User's identifier"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Transaction's type"),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, comment: "Transaction's amount"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Transaction's date"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Transaction's status")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents the Transaction");

            migrationBuilder.CreateIndex(
                name: "IX_CaseOpenings_CaseId",
                table: "CaseOpenings",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseOpenings_ItemId",
                table: "CaseOpenings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseOpenings_UserId",
                table: "CaseOpenings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRewards_CaseId",
                table: "DailyRewards",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRewards_UserId",
                table: "DailyRewards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_ItemId",
                table: "InventoryItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItems_UserId",
                table: "InventoryItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseOpenings");

            migrationBuilder.DropTable(
                name: "DailyRewards");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
