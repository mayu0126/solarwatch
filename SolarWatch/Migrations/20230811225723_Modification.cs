using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarWatch.Migrations
{
    public partial class Modification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SunriseAndSunsetTimes_City",
                table: "SunriseAndSunsetTimes");

            migrationBuilder.CreateIndex(
                name: "IX_SunriseAndSunsetTimes_City",
                table: "SunriseAndSunsetTimes",
                column: "City");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SunriseAndSunsetTimes_City",
                table: "SunriseAndSunsetTimes");

            migrationBuilder.CreateIndex(
                name: "IX_SunriseAndSunsetTimes_City",
                table: "SunriseAndSunsetTimes",
                column: "City",
                unique: true);
        }
    }
}
