using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFree.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_songs_musicians_AuthorId",
                table: "songs");

            migrationBuilder.DropIndex(
                name: "IX_songs_AuthorId",
                table: "songs");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "songs");

            migrationBuilder.CreateTable(
                name: "SongAuthor",
                columns: table => new
                {
                    SongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongAuthor", x => new { x.AuthorId, x.SongId });
                    table.ForeignKey(
                        name: "FK_SongAuthor_musicians_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongAuthor_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongAuthor_SongId",
                table: "SongAuthor",
                column: "SongId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongAuthor");

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorId",
                table: "songs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_songs_AuthorId",
                table: "songs",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_songs_musicians_AuthorId",
                table: "songs",
                column: "AuthorId",
                principalTable: "musicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
