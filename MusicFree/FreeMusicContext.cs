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
        public DbSet<Albumn> albumns { get; set; }
        public DbSet<Musician> musicians { get; set; }
        public DbSet<Song> songs { get; set; }
        public FreeMusicContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            
            modelBuilder.Entity<Song>().HasOne(a => a.Albumn).WithMany(a => a.Songs).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Albumn>().HasOne(a => a.Author).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SongAuthor>()
        .HasKey(bc => new { bc.AuthorId, bc.SongId });

            modelBuilder.Entity<SongAuthor>()
        .HasOne(bc => bc.Song)
        .WithMany(b => b.Authors)
        .HasForeignKey(bc => bc.SongId);
            modelBuilder.Entity<SongAuthor>()
                .HasOne(bc => bc.Author)
                .WithMany(c => c.Songs)
                .HasForeignKey(bc => bc.AuthorId);
        }
    }

}
