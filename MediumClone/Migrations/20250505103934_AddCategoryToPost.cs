using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediumClone.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Posts");
        }
    }
}
