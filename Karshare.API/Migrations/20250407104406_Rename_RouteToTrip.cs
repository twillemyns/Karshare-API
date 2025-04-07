using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karshare.API.Migrations
{
    /// <inheritdoc />
    public partial class Rename_RouteToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassengerLists_Routes_RouteId",
                table: "PassengerLists");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "PassengerLists",
                newName: "TripId");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerLists_RouteId",
                table: "PassengerLists",
                newName: "IX_PassengerLists_TripId");

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EndCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsAnimalAccepted = table.Column<bool>(type: "bit", nullable: false),
                    IsSmokerAccepted = table.Column<bool>(type: "bit", nullable: false),
                    RadioDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsTalkingAccepted = table.Column<bool>(type: "bit", nullable: false),
                    AreKidsAccepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CreatedUserId",
                table: "Trips",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerLists_Trips_TripId",
                table: "PassengerLists",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassengerLists_Trips_TripId",
                table: "PassengerLists");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.RenameColumn(
                name: "TripId",
                table: "PassengerLists",
                newName: "RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_PassengerLists_TripId",
                table: "PassengerLists",
                newName: "IX_PassengerLists_RouteId");

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreKidsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsAnimalAccepted = table.Column<bool>(type: "bit", nullable: false),
                    IsSmokerAccepted = table.Column<bool>(type: "bit", nullable: false),
                    IsTalkingAccepted = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RadioDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StartCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routes_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_CreatedUserId",
                table: "Routes",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PassengerLists_Routes_RouteId",
                table: "PassengerLists",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
