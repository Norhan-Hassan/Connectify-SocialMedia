using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class poke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pokes",
                columns: table => new
                {
                    sourceUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    pokedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokes", x => new { x.pokedUserId, x.sourceUserId });
                    table.ForeignKey(
                        name: "FK_Pokes_AspNetUsers_pokedUserId",
                        column: x => x.pokedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pokes_AspNetUsers_sourceUserId",
                        column: x => x.sourceUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pokes_sourceUserId",
                table: "Pokes",
                column: "sourceUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pokes");
        }
    }
}
