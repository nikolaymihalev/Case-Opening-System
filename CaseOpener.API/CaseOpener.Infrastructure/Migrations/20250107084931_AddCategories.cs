using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CaseOpener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Category's identifier");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Category's identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Category's name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                },
                comment: "Represents the Category");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Weapon" },
                    { 2, "Sticker" },
                    { 3, "Graffiti" },
                    { 4, "Souvenir" },
                    { 5, "Operation" },
                    { 6, "Rare" },
                    { 7, "Event" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Cases_CategoryId",
                table: "Cases",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Categories_CategoryId",
                table: "Cases",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Categories_CategoryId",
                table: "Cases");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Cases_CategoryId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Cases");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEHmbfPIpayyyEgWCXnUEIvnU1VHZr6c3yDTPdCsb7Jr1m7U9dGxERnjN7yF8R7bbJw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a646737-b3ab-4595-9770-2c744e5808c6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIy8S3zuK6nL/f/cymtdVnecQOyXnCpqPNBo02aZgBEgtJMMZE2C5ya2/DtlXW4Lvw==");
        }
    }
}
