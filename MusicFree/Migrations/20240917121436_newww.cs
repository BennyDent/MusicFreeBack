using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFree.Migrations
{
    /// <inheritdoc />
    public partial class newww : Migration
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
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false),
                    MusicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_albumns_musicians_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_albumns_musicians_MusicianId",
                        column: x => x.MusicianId,
                        principalTable: "musicians",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                        name: "FK_songs_musicians_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_albumns_AuthorId",
                table: "albumns",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_albumns_MusicianId",
                table: "albumns",
                column: "MusicianId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_AlbumnId",
                table: "songs",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_AuthorId",
                table: "songs",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "songs");

            migrationBuilder.DropTable(
                name: "albumns");

            migrationBuilder.DropTable(
                name: "musicians");
        }
    }
}
