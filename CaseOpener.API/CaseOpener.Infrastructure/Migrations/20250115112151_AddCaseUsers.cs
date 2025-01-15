using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseOpener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCaseUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseItems_Cases_CaseId",
                table: "CaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseItems_Items_ItemId",
                table: "CaseItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseItems",
                table: "CaseItems");

            migrationBuilder.RenameTable(
                name: "CaseItems",
                newName: "CaseItem");

            migrationBuilder.RenameIndex(
                name: "IX_CaseItems_ItemId",
                table: "CaseItem",
                newName: "IX_CaseItem_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseItems_CaseId",
                table: "CaseItem",
                newName: "IX_CaseItem_CaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseItem",
                table: "CaseItem",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CaseUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Case User identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false, comment: "Case identifier"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User identifier"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Case quanitity")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseUser_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Case User mapping table");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP355Y6e5n5Xi/c7jG2eFOmrcbieUqEUwX6NKfaYuBY86rtZ9pCn9RjtuZeMflWXMA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a646737-b3ab-4595-9770-2c744e5808c6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJ9T9XLDiVHvxccawlABhw64lTMnHFfWO6un/qj9ya3hBJsHSRxPNTGMzKjNxKYU5Q==");

            migrationBuilder.CreateIndex(
                name: "IX_CaseUser_CaseId",
                table: "CaseUser",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseUser_UserId",
                table: "CaseUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseItem_Cases_CaseId",
                table: "CaseItem",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseItem_Items_ItemId",
                table: "CaseItem",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseItem_Cases_CaseId",
                table: "CaseItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseItem_Items_ItemId",
                table: "CaseItem");

            migrationBuilder.DropTable(
                name: "CaseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseItem",
                table: "CaseItem");

            migrationBuilder.RenameTable(
                name: "CaseItem",
                newName: "CaseItems");

            migrationBuilder.RenameIndex(
                name: "IX_CaseItem_ItemId",
                table: "CaseItems",
                newName: "IX_CaseItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseItem_CaseId",
                table: "CaseItems",
                newName: "IX_CaseItems_CaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseItems",
                table: "CaseItems",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CaseItems_Cases_CaseId",
                table: "CaseItems",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseItems_Items_ItemId",
                table: "CaseItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
