using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunityCenterApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMember",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Bookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMember",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Bookings");
        }
    }
}
