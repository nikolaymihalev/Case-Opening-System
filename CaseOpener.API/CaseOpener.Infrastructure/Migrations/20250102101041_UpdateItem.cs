using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseOpener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Item's image url");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAELwhJ+z9byommm5CTbyul7VhYKC84DFkQUxpXApP7T10i4RaroqofARxt7rVuK6bZA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a646737-b3ab-4595-9770-2c744e5808c6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEJaeHnokkMeKNAWI8m8TKKkXpE7s8eGacB9xMAJLugdh8SCmqvnxq2NwmbUydLjp2A==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Items");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Items",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                comment: "Item's image");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "43e21bc5-9592-44bc-aae2-9ca9a16dd5ba",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEOWAQdphO9zl1nuVnG74bkkFfVfU6pGMU++bjaUNth/9zkpC0LtSXnlBsfOaWxaNZg==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "5a646737-b3ab-4595-9770-2c744e5808c6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPrfAaKVdcbrM/LsfBkDW6ar2SM0zDVp9QBbp1tgJxspt0Q+g28niBuYZ+Jz8M2G5w==");
        }
    }
}
