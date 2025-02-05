using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseOpener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseItem_Cases_CaseId",
                table: "CaseItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseItem_Items_ItemId",
                table: "CaseItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseUser_Cases_CaseId",
                table: "CaseUser");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseUser_Users_UserId",
                table: "CaseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseUser",
                table: "CaseUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseItem",
                table: "CaseItem");

            migrationBuilder.RenameTable(
                name: "CaseUser",
                newName: "CaseUsers");

            migrationBuilder.RenameTable(
                name: "CaseItem",
                newName: "CaseItems");

            migrationBuilder.RenameIndex(
                name: "IX_CaseUser_UserId",
                table: "CaseUsers",
                newName: "IX_CaseUsers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseUser_CaseId",
                table: "CaseUsers",
                newName: "IX_CaseUsers_CaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseItem_ItemId",
                table: "CaseItems",
                newName: "IX_CaseItems_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseItem_CaseId",
                table: "CaseItems",
                newName: "IX_CaseItems_CaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseUsers",
                table: "CaseUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseItems",
                table: "CaseItems",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CaseUsers_Cases_CaseId",
                table: "CaseUsers",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseUsers_Users_UserId",
                table: "CaseUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseItems_Cases_CaseId",
                table: "CaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseItems_Items_ItemId",
                table: "CaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseUsers_Cases_CaseId",
                table: "CaseUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseUsers_Users_UserId",
                table: "CaseUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseUsers",
                table: "CaseUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseItems",
                table: "CaseItems");

            migrationBuilder.RenameTable(
                name: "CaseUsers",
                newName: "CaseUser");

            migrationBuilder.RenameTable(
                name: "CaseItems",
                newName: "CaseItem");

            migrationBuilder.RenameIndex(
                name: "IX_CaseUsers_UserId",
                table: "CaseUser",
                newName: "IX_CaseUser_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseUsers_CaseId",
                table: "CaseUser",
                newName: "IX_CaseUser_CaseId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseItems_ItemId",
                table: "CaseItem",
                newName: "IX_CaseItem_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_CaseItems_CaseId",
                table: "CaseItem",
                newName: "IX_CaseItem_CaseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseUser",
                table: "CaseUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseItem",
                table: "CaseItem",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CaseUser_Cases_CaseId",
                table: "CaseUser",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseUser_Users_UserId",
                table: "CaseUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
