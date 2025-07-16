using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFree.Migrations
{
    /// <inheritdoc />
    public partial class NewGenreTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_musicianUsers_musicians_AuthorId",
                table: "musicianUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_musicianUsers",
                table: "musicianUsers");

            migrationBuilder.RenameTable(
                name: "musicianUsers",
                newName: "musician_likes");

            migrationBuilder.RenameIndex(
                name: "IX_musicianUsers_AuthorId",
                table: "musician_likes",
                newName: "IX_musician_likes_AuthorId");

            migrationBuilder.AddColumn<int>(
                name: "listened",
                table: "songsViews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "albumn_index",
                table: "songs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AlbumnId",
                table: "songs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "listened",
                table: "songs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "albumn_type",
                table: "albumns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "listened",
                table: "albumn_views",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "albumn_authors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_musician_likes",
                table: "musician_likes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "playlist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "similar_tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_similar_tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "userAlbumns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userAlbumns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userAlbumns_albumns_AlbumnId",
                        column: x => x.AlbumnId,
                        principalTable: "albumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userMusicians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MusicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userMusicians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_userMusicians_musicians_MusicianId",
                        column: x => x.MusicianId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "genre_albumn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    albumn_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    genre_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre_albumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_genre_albumn_albumns_albumn_id",
                        column: x => x.albumn_id,
                        principalTable: "albumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_genre_albumn_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "genre_musician",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    author_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    genre_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre_musician", x => x.Id);
                    table.ForeignKey(
                        name: "FK_genre_musician_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_genre_musician_musicians_author_id",
                        column: x => x.author_id,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "genre_song",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    song_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    genre_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre_song", x => x.Id);
                    table.ForeignKey(
                        name: "FK_genre_song_genres_genre_id",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_genre_song_songs_song_id",
                        column: x => x.song_id,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "genreUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    genre_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    genreName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    listened = table.Column<int>(type: "int", nullable: false),
                    last_listened = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genreUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_genreUsers_genres_genreName",
                        column: x => x.genreName,
                        principalTable: "genres",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "similar_genre",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_similar_genre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_similar_genre_genres_GenreName",
                        column: x => x.GenreName,
                        principalTable: "genres",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateTable(
                name: "playlistSong",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaylistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlistSong", x => x.Id);
                    table.ForeignKey(
                        name: "FK_playlistSong_playlist_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "playlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_playlistSong_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_albumn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    albumn_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tag_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_albumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tag_albumn_albumns_albumn_id",
                        column: x => x.albumn_id,
                        principalTable: "albumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tag_albumn_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_song",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    song_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tag_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_song", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tag_song_songs_song_id",
                        column: x => x.song_id,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tag_song_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagTagTags",
                columns: table => new
                {
                    similarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tagsName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTagTags", x => new { x.similarId, x.tagsName });
                    table.ForeignKey(
                        name: "FK_TagTagTags_similar_tag_similarId",
                        column: x => x.similarId,
                        principalTable: "similar_tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagTagTags_tags_tagsName",
                        column: x => x.tagsName,
                        principalTable: "tags",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tag_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tagName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    user_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    listened = table.Column<int>(type: "int", nullable: false),
                    last_listened = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagUser_tags_tagName",
                        column: x => x.tagName,
                        principalTable: "tags",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_genre_albumn_albumn_id",
                table: "genre_albumn",
                column: "albumn_id");

            migrationBuilder.CreateIndex(
                name: "IX_genre_albumn_genre_id",
                table: "genre_albumn",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_genre_musician_author_id",
                table: "genre_musician",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_genre_musician_genre_id",
                table: "genre_musician",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_genre_song_genre_id",
                table: "genre_song",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_genre_song_song_id",
                table: "genre_song",
                column: "song_id");

            migrationBuilder.CreateIndex(
                name: "IX_genreUsers_genreName",
                table: "genreUsers",
                column: "genreName");

            migrationBuilder.CreateIndex(
                name: "IX_playlistSong_PlaylistId",
                table: "playlistSong",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_playlistSong_SongId",
                table: "playlistSong",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_similar_genre_GenreName",
                table: "similar_genre",
                column: "GenreName");

            migrationBuilder.CreateIndex(
                name: "IX_tag_albumn_albumn_id",
                table: "tag_albumn",
                column: "albumn_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_albumn_tag_id",
                table: "tag_albumn",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_song_song_id",
                table: "tag_song",
                column: "song_id");

            migrationBuilder.CreateIndex(
                name: "IX_tag_song_tag_id",
                table: "tag_song",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_TagTagTags_tagsName",
                table: "TagTagTags",
                column: "tagsName");

            migrationBuilder.CreateIndex(
                name: "IX_TagUser_tagName",
                table: "TagUser",
                column: "tagName");

            migrationBuilder.CreateIndex(
                name: "IX_userAlbumns_AlbumnId",
                table: "userAlbumns",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_userMusicians_MusicianId",
                table: "userMusicians",
                column: "MusicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_musician_likes_musicians_AuthorId",
                table: "musician_likes",
                column: "AuthorId",
                principalTable: "musicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_musician_likes_musicians_AuthorId",
                table: "musician_likes");

            migrationBuilder.DropTable(
                name: "genre_albumn");

            migrationBuilder.DropTable(
                name: "genre_musician");

            migrationBuilder.DropTable(
                name: "genre_song");

            migrationBuilder.DropTable(
                name: "genreUsers");

            migrationBuilder.DropTable(
                name: "playlistSong");

            migrationBuilder.DropTable(
                name: "similar_genre");

            migrationBuilder.DropTable(
                name: "tag_albumn");

            migrationBuilder.DropTable(
                name: "tag_song");

            migrationBuilder.DropTable(
                name: "TagTagTags");

            migrationBuilder.DropTable(
                name: "TagUser");

            migrationBuilder.DropTable(
                name: "userAlbumns");

            migrationBuilder.DropTable(
                name: "userMusicians");

            migrationBuilder.DropTable(
                name: "playlist");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "similar_tag");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_musician_likes",
                table: "musician_likes");

            migrationBuilder.DropColumn(
                name: "listened",
                table: "songsViews");

            migrationBuilder.DropColumn(
                name: "listened",
                table: "songs");

            migrationBuilder.DropColumn(
                name: "albumn_type",
                table: "albumns");

            migrationBuilder.DropColumn(
                name: "listened",
                table: "albumn_views");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "albumn_authors");

            migrationBuilder.RenameTable(
                name: "musician_likes",
                newName: "musicianUsers");

            migrationBuilder.RenameIndex(
                name: "IX_musician_likes_AuthorId",
                table: "musicianUsers",
                newName: "IX_musicianUsers_AuthorId");

            migrationBuilder.AlterColumn<int>(
                name: "albumn_index",
                table: "songs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "AlbumnId",
                table: "songs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_musicianUsers",
                table: "musicianUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_musicianUsers_musicians_AuthorId",
                table: "musicianUsers",
                column: "AuthorId",
                principalTable: "musicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
