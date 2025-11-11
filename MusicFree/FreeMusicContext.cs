using EF6TempTableKit.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MusicFree.Models;
using MusicFree.Models.GenreAndName;
using System;
using System.Collections.Generic;



using System.Reflection.Emit;
namespace MusicFree
{
  
    public class FreeMusicContext :DbContext
    {   
        public DbSet<SongAuthor> song_authors {  get; set; }
        public DbSet<AlbumnAuthor> albumn_authors { get; set; } 
        public DbSet<Albumn> albumns { get; set; }
        public DbSet<Musician> musicians { get; set; }
        public DbSet<Song> songs { get; set; }
        public DbSet<SongViews> songsViews { get; set; }
     
        public DbSet<SearchModel> searches { get; set; }

        public DbSet<Playlist> playlist { get; set; }

        public DbSet<PlaylistSong> playlistSong { get; set; }
        public DbSet<AlbumnViews> albumn_views { get; set; }

        public DbSet <UserSong> likes { get; set; }

        public DbSet<UserMusician> musician_likes { get; set; }

        public DbSet<GenretoMusician> genre_musician {  get; set; }

        public DbSet<GenreUser> genreUsers { get; set; }

        public DbSet<UserAlbumn> userAlbumns { get; set; }

        public DbSet<Tags> tags { get; set; }
        public DbSet<Genre> genres { get; set; }
      
        public DbSet<MusicianView> userMusicians { get; set; }
        public DbSet<MusicianLastSearch> musicianlastSearches { get; set; }


        public DbSet<AlbumnLastSearch> albumnlastSearches { get; set; }

        public DbSet<SongLastSearch> songlastSearches    { get; set; }
        public DbSet<GenretoSong> genre_song { get; set; }

        public DbSet<GenretoAlbumn> genre_albumn { get; set; }

        public DbSet<TagtoAlbumn> tag_albumn { get; set; }

        public DbSet<TagtoSong> tag_song { get; set; }

        public DbSet<GenreGenre> similar_genre { get; set; }

        public DbSet<User> user { get; set; }

        public DbSet<UserRadio> user_radio { get; set; }

        public DbSet<TagTag> similar_tag { get; set; }

        

        public FreeMusicContext(DbContextOptions options) : base(options) { }



   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // Each User can have many UserClaims





            modelBuilder.Entity<Song>().Navigation(a => a.Main_Author).AutoInclude();
            modelBuilder.Entity<Song>().Navigation(a=>a.Albumn).AutoInclude();
            modelBuilder.Entity<Albumn>().Navigation(a => a.Main_Author).AutoInclude();
            modelBuilder.Entity<Albumn>().Navigation(a => a.Songs).AutoInclude();
           modelBuilder.Entity<Musician>().Property(a => a.auto_increment_index).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Song>().HasOne(a => a.Main_Author).WithMany(a => a.Songs);
            modelBuilder.Entity<Albumn>().Property(a => a.auto_increment_index).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Song>().Property(a => a.auto_increment_index).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Tags>().Property(a => a.auto_increment_index).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<Genre>().Property(a => a.auto_increment_index).ValueGeneratedOnAdd().Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            modelBuilder.Entity<PlaylistSong>().HasOne(a => a.Song).WithMany(a => a.playllists).HasForeignKey(a=> a.SongId);
            modelBuilder.Entity<PlaylistSong>().HasOne(a=> a.Playlist).WithMany(a=> a.songs).HasForeignKey(songs => songs.PlaylistId);
         
            modelBuilder.Entity<UserMusician>().HasOne(b => b.author).WithMany(a => a.liked_by).HasForeignKey(b => b.AuthorId);
            modelBuilder.Entity<UserMusician>().HasOne(a => a.user).WithMany(a => a.author_subcription).HasForeignKey(b => b.UserId);

            modelBuilder.Entity<UserAlbumn>().HasOne(a => a.Albumn).WithMany(a => a.liked_by).HasForeignKey(a => a.AlbumnId);
            modelBuilder.Entity<UserAlbumn>().HasOne(a => a.user).WithMany(a=>a.liked_albumns).HasForeignKey(a=> a.UserId);

            modelBuilder.Entity<MusicianView>().HasOne(a => a.user).WithMany(a => a.authors_view).HasForeignKey(a=> a.UserId);
            modelBuilder.Entity<MusicianView>().HasOne(b => b.author).WithMany(a => a.musician_views).HasForeignKey(b => b.MusicianId);

            modelBuilder.Entity<TagtoAlbumn>().HasOne(a => a.albumn).WithMany(a=>a.tags).HasForeignKey(a=> a.albumn_id);
            modelBuilder.Entity<TagtoAlbumn>().HasOne(a => a.tag).WithMany(a => a.albumns).HasForeignKey(a => a.tag_id);

            modelBuilder.Entity<TagtoSong>().HasOne(a => a.song).WithMany(a => a.tags).HasForeignKey(a => a.song_id);
            modelBuilder.Entity<TagtoSong>().HasOne(a => a.tag).WithMany(a => a.song).HasForeignKey(a => a.tag_id);
            modelBuilder.Entity<UserSong>().HasOne(a => a.song).WithMany(a => a.liked_by).HasForeignKey(a => a.SongId);
            modelBuilder.Entity<UserSong>().HasOne(a => a.user).WithMany(a => a.song_likes).HasForeignKey(a => a.UserId);

            modelBuilder.Entity<AlbumnLastSearch>().HasOne(a => a.Albumn).WithMany(a => a.lastSearch).HasForeignKey(a => a.AlbumnId);
            modelBuilder.Entity<MusicianLastSearch>().HasOne(a => a.Author).WithMany(a => a.lastSearch).HasForeignKey(a=> a.AuthorId);
            modelBuilder.Entity<SongLastSearch>().HasOne(a => a.Song).WithMany(a => a.lastSearch).HasForeignKey(a => a.SongId);
            modelBuilder.Entity<GenretoSong>().HasOne(a => a.song).WithMany(a => a.genres).HasForeignKey(a => a.song_id);
            modelBuilder.Entity<GenretoSong>().HasOne(a => a.genre).WithMany(a => a.song).HasForeignKey(a => a.genre_id);
            modelBuilder.Entity<GenretoAlbumn>().HasOne(a=>a.albumn).WithMany(a => a.genres).HasForeignKey(a => a.albumn_id);
            modelBuilder.Entity<GenretoAlbumn>().HasOne(a => a.genre).WithMany(a => a.albumns).HasForeignKey(a => a.genre_id);


            modelBuilder.Entity<GenretoMusician>().HasOne(a => a.genre).WithMany(a=> a.authors).HasForeignKey(a=> a.genre_id);
            modelBuilder.Entity<GenretoMusician>().HasOne(a => a.author).WithMany(a => a.genres).HasForeignKey(a=> a.author_id);



           


            modelBuilder.Entity<AlbumnViews>().HasOne(a => a.user).WithMany(a => a.albumn_views).HasForeignKey(a=>a.UserId);
            modelBuilder.Entity<AlbumnViews>().HasOne(b => b.albumn).WithMany(a => a.albumn_views).HasForeignKey(b => b.AlbumnId);
          

        

            modelBuilder.Entity<SongViews>().HasOne(b => b.song).WithMany(a => a.song_views).HasForeignKey(b => b.SongId);
            modelBuilder.Entity<SongViews>().HasOne(a => a.user).WithMany(a => a.song_views).HasForeignKey(b=>b.UserId);
            modelBuilder.Entity<Song>().HasOne(b => b.Albumn).WithMany(a=>a.Songs).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Albumn>().HasOne(a => a.Main_Author).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AlbumnAuthor>()
           .HasOne(bc => bc.Albumn)
       .WithMany(b => b.Extra_Authors)
       .HasForeignKey(bc => bc.AlbumnId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AlbumnAuthor>()
       .HasOne(bc => bc.Author)
       .WithMany(b => b.collaboration_albumns)
       .HasForeignKey(bc => bc.AuthorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AlbumnAuthor>()
.HasKey(bc => new { bc.AuthorId, bc.AlbumnId });
            modelBuilder.Entity<SongAuthor>()
        .HasKey(bc => new { bc.AuthorId, bc.SongId });
           
            modelBuilder.Entity<SongAuthor>()
        .HasOne(bc => bc.Song)
        .WithMany(b => b.extra_authors)
        .HasForeignKey(bc => bc.SongId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SongAuthor>()
                .HasOne(bc => bc.Author)
                .WithMany(c => c.collaboration_songs)
                .HasForeignKey(bc => bc.AuthorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Song>().HasOne(a=>a.Main_Author).WithMany(a=>a.Songs).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Albumn>().HasOne(a => a.Main_Author).WithMany(a=> a.Albumns).OnDelete(DeleteBehavior.Restrict);

        }



      




    }
    }

