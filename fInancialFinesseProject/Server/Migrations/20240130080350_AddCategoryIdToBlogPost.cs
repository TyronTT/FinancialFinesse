using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fInancialFinesseProject.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToBlogPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "BlogPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BlogPosts");
        }
    }
}
