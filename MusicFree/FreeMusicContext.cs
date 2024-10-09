using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using MusicFree.Models;
using System.Reflection.Emit;
namespace MusicFree
{
    public class FreeMusicContext : DbContext
    {   
        public DbSet<SongAuthor> song_authors {  get; set; }
        public DbSet<AlbumnAuthor> albumn_authors { get; set; } 
        public DbSet<Albumn> albumns { get; set; }
        public DbSet<Musician> musicians { get; set; }
        public DbSet<Song> songs { get; set; }
        public FreeMusicContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

            
            modelBuilder.Entity<Song>().HasOne(a => a.Albumn).WithMany(a => a.Songs).OnDelete(DeleteBehavior.Restrict);
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
