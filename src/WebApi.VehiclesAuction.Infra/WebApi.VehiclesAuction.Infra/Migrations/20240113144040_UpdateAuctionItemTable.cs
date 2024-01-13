using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.VehiclesAuction.Infra.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuctionItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "START_AT",
                table: "AUCTION_ITEM",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "END_AT",
                table: "AUCTION_ITEM",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "START_AT",
                table: "AUCTION_ITEM",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "END_AT",
                table: "AUCTION_ITEM",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time without time zone");
        }
    }
}
