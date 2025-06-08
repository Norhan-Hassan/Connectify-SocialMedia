using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class postreactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_Groups_GroupID",
                table: "UserConnections");

            migrationBuilder.AlterColumn<int>(
                name: "GroupID",
                table: "UserConnections",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ReactedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    applicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    postId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reactions_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reactions_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_applicationUserId",
                table: "Reactions",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_postId",
                table: "Reactions",
                column: "postId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_Groups_GroupID",
                table: "UserConnections",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_Groups_GroupID",
                table: "UserConnections");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.AlterColumn<int>(
                name: "GroupID",
                table: "UserConnections",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_Groups_GroupID",
                table: "UserConnections",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");
        }
    }
}
