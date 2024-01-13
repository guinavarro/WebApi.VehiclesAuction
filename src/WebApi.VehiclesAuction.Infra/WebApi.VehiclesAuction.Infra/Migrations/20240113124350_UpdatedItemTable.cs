using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.VehiclesAuction.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AUCTION_ITEM_ID",
                table: "ITEM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AUCTION_ITEM_ID",
                table: "ITEM",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
