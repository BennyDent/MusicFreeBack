using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicFree.Migrations
{
    /// <inheritdoc />
    public partial class wffake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    auto_increment_index = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "musicians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cover_src = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    auto_increment_index = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musicians", x => x.Id);
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
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    auto_increment_index = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "user_radio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    radio_index = table.Column<int>(type: "int", nullable: false),
                    same_author_possibility = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_radio", x => x.Id);
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
                name: "albumns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Main_AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_visible = table.Column<bool>(type: "bit", nullable: false),
                    cover_src = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    albumn_type = table.Column<int>(type: "int", nullable: false),
                    release_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    auto_increment_index = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
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
                name: "musicianlastSearches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_searched = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musicianlastSearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_musicianlastSearches_musicians_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
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

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    last_search = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RadioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    confirm_code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_user_radio_RadioId",
                        column: x => x.RadioId,
                        principalTable: "user_radio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "albumn_authors",
                columns: table => new
                {
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "albumnlastSearches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_searched = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albumnlastSearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_albumnlastSearches_albumns_AlbumnId",
                        column: x => x.AlbumnId,
                        principalTable: "albumns",
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
                name: "searches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_albumn = table.Column<bool>(type: "bit", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_searches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_searches_albumns_AlbumnId",
                        column: x => x.AlbumnId,
                        principalTable: "albumns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_searches_musicians_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "songs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    file_src = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Main_AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    albumn_index = table.Column<int>(type: "int", nullable: false),
                    listened = table.Column<int>(type: "int", nullable: false),
                    cover_src = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    auto_increment_index = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_songs_albumns_AlbumnId",
                        column: x => x.AlbumnId,
                        principalTable: "albumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_songs_musicians_Main_AuthorId",
                        column: x => x.Main_AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "albumn_views",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_albumn_views", x => x.Id);
                    table.ForeignKey(
                        name: "FK_albumn_views_albumns_AlbumnId",
                        column: x => x.AlbumnId,
                        principalTable: "albumns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_albumn_views_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "musician_likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_musician_likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_musician_likes_musicians_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "musicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_musician_likes_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    authorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_playlist_user_authorId",
                        column: x => x.authorId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "userAlbumns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AlbumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_userAlbumns_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userMusicians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MusicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_userMusicians_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
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
                name: "likes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_likes_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_likes_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "song_authors",
                columns: table => new
                {
                    SongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "songlastSearches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_searched = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songlastSearches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_songlastSearches_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "songsViews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_songsViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_songsViews_songs_SongId",
                        column: x => x.SongId,
                        principalTable: "songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_songsViews_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
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

            migrationBuilder.CreateIndex(
                name: "IX_albumn_authors_AlbumnId",
                table: "albumn_authors",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_albumn_views_AlbumnId",
                table: "albumn_views",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_albumn_views_UserId",
                table: "albumn_views",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_albumnlastSearches_AlbumnId",
                table: "albumnlastSearches",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_albumns_Main_AuthorId",
                table: "albumns",
                column: "Main_AuthorId");

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
                name: "IX_likes_SongId",
                table: "likes",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_UserId",
                table: "likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_musician_likes_AuthorId",
                table: "musician_likes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_musician_likes_UserId",
                table: "musician_likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_musicianlastSearches_AuthorId",
                table: "musicianlastSearches",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_authorId",
                table: "playlist",
                column: "authorId");

            migrationBuilder.CreateIndex(
                name: "IX_playlistSong_PlaylistId",
                table: "playlistSong",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_playlistSong_SongId",
                table: "playlistSong",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_searches_AlbumnId",
                table: "searches",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_searches_AuthorId",
                table: "searches",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_similar_genre_GenreName",
                table: "similar_genre",
                column: "GenreName");

            migrationBuilder.CreateIndex(
                name: "IX_song_authors_SongId",
                table: "song_authors",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_songlastSearches_SongId",
                table: "songlastSearches",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_AlbumnId",
                table: "songs",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_songs_Main_AuthorId",
                table: "songs",
                column: "Main_AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_songsViews_SongId",
                table: "songsViews",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_songsViews_UserId",
                table: "songsViews",
                column: "UserId");

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
                name: "IX_user_RadioId",
                table: "user",
                column: "RadioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userAlbumns_AlbumnId",
                table: "userAlbumns",
                column: "AlbumnId");

            migrationBuilder.CreateIndex(
                name: "IX_userAlbumns_UserId",
                table: "userAlbumns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_userMusicians_MusicianId",
                table: "userMusicians",
                column: "MusicianId");

            migrationBuilder.CreateIndex(
                name: "IX_userMusicians_UserId",
                table: "userMusicians",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "albumn_authors");

            migrationBuilder.DropTable(
                name: "albumn_views");

            migrationBuilder.DropTable(
                name: "albumnlastSearches");

            migrationBuilder.DropTable(
                name: "genre_albumn");

            migrationBuilder.DropTable(
                name: "genre_musician");

            migrationBuilder.DropTable(
                name: "genre_song");

            migrationBuilder.DropTable(
                name: "genreUsers");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "musician_likes");

            migrationBuilder.DropTable(
                name: "musicianlastSearches");

            migrationBuilder.DropTable(
                name: "playlistSong");

            migrationBuilder.DropTable(
                name: "searches");

            migrationBuilder.DropTable(
                name: "similar_genre");

            migrationBuilder.DropTable(
                name: "song_authors");

            migrationBuilder.DropTable(
                name: "songlastSearches");

            migrationBuilder.DropTable(
                name: "songsViews");

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
                name: "songs");

            migrationBuilder.DropTable(
                name: "similar_tag");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "albumns");

            migrationBuilder.DropTable(
                name: "user_radio");

            migrationBuilder.DropTable(
                name: "musicians");
        }
    }
}
