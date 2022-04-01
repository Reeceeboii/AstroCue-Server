using Microsoft.EntityFrameworkCore.Migrations;

namespace AstroCue.Server.Migrations
{
    public partial class ReportEntityAltAz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "HorizontalCoordinates_Altitude",
                table: "Reports",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HorizontalCoordinates_Azimuth",
                table: "Reports",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorizontalCoordinates_Altitude",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "HorizontalCoordinates_Azimuth",
                table: "Reports");
        }
    }
}
