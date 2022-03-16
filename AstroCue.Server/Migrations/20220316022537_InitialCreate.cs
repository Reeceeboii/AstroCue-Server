using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AstroCue.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AstroCueUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstroCueUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AstronomicalObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogueIdentifier = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightAscension_Hours = table.Column<int>(type: "int", nullable: false),
                    RightAscension_Minutes = table.Column<int>(type: "int", nullable: false),
                    RightAscension_Seconds = table.Column<double>(type: "float", nullable: false),
                    Declination_Degrees = table.Column<int>(type: "int", nullable: false),
                    Declination_Minutes = table.Column<int>(type: "int", nullable: false),
                    Declination_Seconds = table.Column<double>(type: "float", nullable: false),
                    ApparentMagnitude = table.Column<float>(type: "real", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartOfMultipleSystem = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AstronomicalObjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObservationLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    BortleScaleValue = table.Column<int>(type: "int", nullable: false),
                    BortleDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AstroCueUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObservationLocations_AstroCueUsers_AstroCueUserId",
                        column: x => x.AstroCueUserId,
                        principalTable: "AstroCueUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObservationLocationId = table.Column<int>(type: "int", nullable: false),
                    AstronomicalObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_AstronomicalObjects_AstronomicalObjectId",
                        column: x => x.AstronomicalObjectId,
                        principalTable: "AstronomicalObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Observations_ObservationLocations_ObservationLocationId",
                        column: x => x.ObservationLocationId,
                        principalTable: "ObservationLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObservationLocationId = table.Column<int>(type: "int", nullable: true),
                    AstroCueUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AstroCueUsers_AstroCueUserId",
                        column: x => x.AstroCueUserId,
                        principalTable: "AstroCueUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_ObservationLocations_ObservationLocationId",
                        column: x => x.ObservationLocationId,
                        principalTable: "ObservationLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObservationLocations_AstroCueUserId",
                table: "ObservationLocations",
                column: "AstroCueUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_AstronomicalObjectId",
                table: "Observations",
                column: "AstronomicalObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Observations_ObservationLocationId",
                table: "Observations",
                column: "ObservationLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AstroCueUserId",
                table: "Reports",
                column: "AstroCueUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ObservationLocationId",
                table: "Reports",
                column: "ObservationLocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "AstronomicalObjects");

            migrationBuilder.DropTable(
                name: "ObservationLocations");

            migrationBuilder.DropTable(
                name: "AstroCueUsers");
        }
    }
}
