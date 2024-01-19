using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class Small : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "movies_fk0",
                table: "movies");

            migrationBuilder.DropForeignKey(
                name: "screenings_fk0",
                table: "screenings");

            migrationBuilder.AddForeignKey(
                name: "movies_fk0",
                table: "movies",
                column: "content_admin_id",
                principalTable: "content_admin",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "screenings_fk0",
                table: "screenings",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "movies_fk0",
                table: "movies");

            migrationBuilder.DropForeignKey(
                name: "screenings_fk0",
                table: "screenings");

            migrationBuilder.AddForeignKey(
                name: "movies_fk0",
                table: "movies",
                column: "content_admin_id",
                principalTable: "content_admin",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "screenings_fk0",
                table: "screenings",
                column: "movie_id",
                principalTable: "movies",
                principalColumn: "id");
        }
    }
}
