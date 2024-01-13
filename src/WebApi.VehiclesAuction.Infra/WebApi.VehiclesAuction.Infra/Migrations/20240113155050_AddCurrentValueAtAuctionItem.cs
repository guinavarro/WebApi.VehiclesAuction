using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.VehiclesAuction.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentValueAtAuctionItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CURRENT_VALUE",
                table: "AUCTION_ITEM",
                type: "numeric(10,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CURRENT_VALUE",
                table: "AUCTION_ITEM");
        }
    }
}
