using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class addgroupEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "UserConnections",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_GroupID",
                table: "UserConnections",
                column: "GroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_Groups_GroupID",
                table: "UserConnections",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_Groups_GroupID",
                table: "UserConnections");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_GroupID",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "UserConnections");
        }
    }
}
