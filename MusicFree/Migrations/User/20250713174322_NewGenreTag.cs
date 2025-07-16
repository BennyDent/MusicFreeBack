using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFree.Migrations.User
{
    /// <inheritdoc />
    public partial class NewGenreTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "radios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    radio_history = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    radio_stack = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    radio_index = table.Column<int>(type: "int", nullable: false),
                    same_author_possibility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_radios_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_radios_user_id",
                table: "radios",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "radios");
        }
    }
}
