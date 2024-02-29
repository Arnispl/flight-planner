using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightPlanner.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAirportCodeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AirportCode",
                table: "Airports",
                newName: "airport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "airport",
                table: "Airports",
                newName: "AirportCode");
        }
    }
}
