using Microsoft.EntityFrameworkCore.Migrations;

namespace AstroCue.Server.Migrations
{
    public partial class ObservationLogReportId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LogForReportId",
                table: "ObservationLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogForReportId",
                table: "ObservationLogs");
        }
    }
}
