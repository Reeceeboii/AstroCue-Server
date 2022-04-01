using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AstroCue.Server.Migrations
{
    public partial class ReportEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AstronomicalObjectName",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BestTimeToObserve",
                table: "Reports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MoreInformationUrl",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeatherForecast_CloudCoveragePercent",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeatherForecast_Description",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeatherForecast_HumidityPercent",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "WeatherForecast_ProbabilityOfPrecipitation",
                table: "Reports",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WeatherForecast_Sunrise",
                table: "Reports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WeatherForecast_Sunset",
                table: "Reports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeatherForecast_TemperatureCelcius",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "WeatherForecast_WindSpeedMetersPerSec",
                table: "Reports",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AstronomicalObjectName",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "BestTimeToObserve",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MoreInformationUrl",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "WeatherForecast_CloudCoveragePercent",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "WeatherForecast_Description",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "WeatherForecast_HumidityPercent",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "WeatherForecast_ProbabilityOfPrecipitation",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "WeatherForecast_Sunrise",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "WeatherForecast_Sunset",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "WeatherForecast_TemperatureCelcius",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "WeatherForecast_WindSpeedMetersPerSec",
                table: "Reports");
        }
    }
}
