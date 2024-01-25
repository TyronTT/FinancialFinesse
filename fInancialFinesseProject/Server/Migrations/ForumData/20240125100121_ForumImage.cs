using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fInancialFinesseProject.Server.Migrations.ForumData
{
    /// <inheritdoc />
    public partial class ForumImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ForumPosts",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ForumPosts");
        }
    }
}
