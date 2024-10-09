using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFree.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "musicians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musicians", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "albumns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Main_AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_albumns_musicians_Main_AuthorId",
                        column: x => x.Main_AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "albumn_authors",
                columns: table => new
                {
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albumn_authors", x => new { x.AuthorId, x.AlbumnId });
                    table.ForeignKey(
                        name: "FK_albumn_authors_albumns_AlbumnId",
                        column: x => x.AlbumnId,
                        principalTable: "albumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_albumn_authors_musicians_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Main_AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    albumn_index = table.Column<int>(type: "int", nullable: true),
                    song_filename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_songs_albumns_AlbumnId",
                        column: x => x.AlbumnId,
                        principalTable: "albumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_songs_musicians_Main_AuthorId",
                        column: x => x.Main_AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "song_authors",
                columns: table => new
                {
                    SongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_song_authors", x => new { x.AuthorId, x.SongId });
                    table.ForeignKey(
                        name: "FK_song_authors_musicians_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_song_authors_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_albumn_authors_AlbumnId",
                table: "albumn_authors",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_albumns_Main_AuthorId",
                table: "albumns",
                column: "Main_AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_song_authors_SongId",
                table: "song_authors",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_AlbumnId",
                table: "songs",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_Main_AuthorId",
                table: "songs",
                column: "Main_AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "albumn_authors");

            migrationBuilder.DropTable(
                name: "song_authors");

            migrationBuilder.DropTable(
                name: "songs");

            migrationBuilder.DropTable(
                name: "albumns");

            migrationBuilder.DropTable(
                name: "musicians");
        }
    }
}
