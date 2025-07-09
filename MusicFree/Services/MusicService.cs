using MusicFree.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using Microsoft.AspNetCore.Identity;
namespace MusicFree.Services { 

public class MusicService
{
       
       
    public MusicService()
        {
            
            
        }

       

      
        
        public List<AuthorReturn> ExtraAuthorsReturn(ICollection<SongAuthor> list)
        {
            var authors_list = new List<AuthorReturn>();
            foreach (var author in list)
            {
                authors_list.Add(new AuthorReturn(author.AuthorId, author.Author.Name));
            }
            return authors_list;
        }

        public AlbumnRethurn AlbumnReturn(Albumn albumn)
        {
           var for_return= new AlbumnRethurn(albumn.Id, albumn.Name, new AuthorReturn(albumn.Main_Author.Id, albumn.Main_Author.Name), ExtraAuthorsAlbumnReturn(albumn.Extra_Authors),albumn.cover_filename);
          
            return for_return;
        }

        public SongReturn SongToSongReturn(Song song, User user) {
            var is_liked = false;
            Console.WriteLine(user == null);
            if (user != null) {
            if (user.song_likes.Where(a=> a==song.Id).Any())
            {
                    Console.WriteLine(66);
                is_liked=true;
                    Console.WriteLine(is_liked);
                } }
            Console.WriteLine(is_liked);

            return new SongReturn(song.Id, new AuthorReturn(song.Main_Author.Id, song.Main_Author.Name), ExtraAuthorsReturn(song.extra_authors),
                    song.song_filename, song.Name, is_liked
                    );
        
        }


        public List<SongReturn> SongstoSongReturns(List<Song> songs, User user) { 
        
        var songs_return = new List<SongReturn>();

            foreach (Song song in songs)
            {
            songs_return.Add(SongToSongReturn(song, user)); 
            }
            return songs_return;
        }


        public int AlbumnViews(Albumn a)
        {
            var songs = a.Songs;
            var views = 0;
            foreach (var song in songs)
            {//
                views =  views + song.song_views.Count();
            }
            return views;

        }

        public List<AuthorReturn> ExtraAuthorsAlbumnReturn(ICollection<AlbumnAuthor> list)
        {
            var authors_list = new List<AuthorReturn>();
            if(list.Count == 0)
            {
                return authors_list;
            }
            foreach (var author in list)
            {
                authors_list.Add(new AuthorReturn(author.AuthorId, author.Author.Name));
            }
            return authors_list;
        }

        public List<AuthorReturn> MusicianToAuthorReturn(List<Musician> author)
        {
            var musician_list = new List<AuthorReturn>();
            foreach(var m in author)
            {
 var musician = new AuthorReturn(m.Id, m.Name);
            musician.src = m.img_filename;
                musician_list.Add(musician);    
            }
           

            return musician_list ;
        }
    }  
}