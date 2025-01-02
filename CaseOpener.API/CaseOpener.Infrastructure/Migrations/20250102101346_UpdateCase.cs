using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaseOpener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Cases");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Case's image");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Cases");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Cases",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                comment: "Case's image");

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
    }
}
