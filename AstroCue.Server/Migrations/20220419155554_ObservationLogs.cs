using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AstroCue.Server.Migrations
{
    public partial class ObservationLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AstronomicalObjectReportIsTargeting",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ObservationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextualDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Observer = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    WeatherForecast_CloudCoveragePercent = table.Column<int>(type: "int", nullable: false),
                    WeatherForecast_TemperatureCelcius = table.Column<int>(type: "int", nullable: false),
                    WeatherForecast_HumidityPercent = table.Column<int>(type: "int", nullable: false),
                    WeatherForecast_WindSpeedMetersPerSec = table.Column<float>(type: "real", nullable: false),
                    WeatherForecast_ProbabilityOfPrecipitation = table.Column<float>(type: "real", nullable: false),
                    WeatherForecast_Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeatherForecast_Sunset = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeatherForecast_Sunrise = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObservationLocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObservationLocationLatitude = table.Column<float>(type: "real", nullable: false),
                    ObservationLocationLongitude = table.Column<float>(type: "real", nullable: false),
                    ObservedAstronomicalObjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalculatedBestTimeToObserveUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HorizontalCoordinates_Altitude = table.Column<float>(type: "real", nullable: false),
                    HorizontalCoordinates_Azimuth = table.Column<float>(type: "real", nullable: false),
                    TypeOfObservation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTaken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AstroCueUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservationLogs_AstroCueUsers_AstroCueUserId",
                        column: x => x.AstroCueUserId,
                        principalTable: "AstroCueUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObservationLogs_AstroCueUserId",
                table: "ObservationLogs",
                column: "AstroCueUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObservationLogs");

            migrationBuilder.DropColumn(
                name: "AstronomicalObjectReportIsTargeting",
                table: "Reports");
        }
    }
}
