using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using MusicFree.Models;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<MusicianUser> musicianUsers { get; set; }
        public DbSet<SearchModel> searches { get; set; }

        public DbSet<Playlist> playlist { get; set; }

        public DbSet<PlaylistSong> playlistSong { get; set; }
        public DbSet<AlbumnViews> albumn_views { get; set; }

        public DbSet <UserSong> likes { get; set; }
        public FreeMusicContext(DbContextOptions options) : base(options) { }

        public DbSet<Tags> tags { get; set; }
        public DbSet<Genres> genres { get; set; }
        public DbSet<AppearedSongs> apeared_songs { get; set; }
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            // Each User can have many UserClaims

            modelBuilder.Entity<PlaylistSong>().HasOne(a => a.Song).WithMany(a => a.playllists).HasForeignKey(a=> a.SongId);
            modelBuilder.Entity<PlaylistSong>().HasOne(a=> a.Playlist).WithMany(a=> a.songs).HasForeignKey(songs => songs.PlaylistId);
         
            modelBuilder.Entity<MusicianUser>().HasOne(b => b.author).WithMany(a => a.liked_by).HasForeignKey(b => b.AuthorId);

          

          

          


            modelBuilder.Entity<AlbumnViews>().HasOne(b => b.albumn).WithMany(a => a.albumn_views).HasForeignKey(b => b.AlbumnId);

           modelBuilder.Entity<Song>().HasMany(a =>a.liked_by).WithOne(a => a.Song).HasForeignKey(a => a.SongId);

            modelBuilder.Entity<SongViews>().HasOne(b => b.song).WithMany(a => a.song_views).HasForeignKey(b => b.SongId);
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

