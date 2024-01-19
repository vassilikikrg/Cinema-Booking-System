using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaTicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBehaviourOnDeleteMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "movies_fk0",
                table: "movies");

            migrationBuilder.AddForeignKey(
                name: "movies_fk0",
                table: "movies",
                column: "content_admin_id",
                principalTable: "content_admin",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "movies_fk0",
                table: "movies");

            migrationBuilder.AddForeignKey(
                name: "movies_fk0",
                table: "movies",
                column: "content_admin_id",
                principalTable: "content_admin",
                principalColumn: "id");
        }
    }
}
