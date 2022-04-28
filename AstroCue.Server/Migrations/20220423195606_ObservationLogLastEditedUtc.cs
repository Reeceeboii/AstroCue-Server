using Microsoft.EntityFrameworkCore.Migrations;

namespace AstroCue.Server.Migrations
{
    public partial class ObservationLogLastEditedUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTaken",
                table: "ObservationLogs",
                newName: "DateLastEditedUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateLastEditedUtc",
                table: "ObservationLogs",
                newName: "DateTaken");
        }
    }
}
