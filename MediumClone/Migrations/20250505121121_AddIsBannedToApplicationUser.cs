using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediumClone.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBannedToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "AspNetUsers");
        }
    }
}
