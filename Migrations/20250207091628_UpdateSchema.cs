using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelRecommendationsAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransportMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccommodationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuisineImportance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourismStyle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SceneryVibe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFeedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DestinationId = table.Column<int>(type: "int", nullable: false),
                    FeedbackId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFeedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BudgetLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransportMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredDestinationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredActivities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferredAccommodation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuisineImportance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourismStyle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TravelGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SceneryVibe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Destinations");

            migrationBuilder.DropTable(
                name: "FeedbackTypes");

            migrationBuilder.DropTable(
                name: "UserFeedbacks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
