using MusicFree.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using Microsoft.AspNetCore.Identity;
using MusicFree.Models.DataReturnModel;
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
                authors_list.Add(new AuthorReturn(author.Author));
            }
            return authors_list;
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

        public List<AuthorReturn> MusicianstoAuthorReturn(List<Musician> authors)
        {
            var new_author = new List<AuthorReturn>();
            foreach (var author in authors)
            {
                new_author.Add(new AuthorReturn(author));
            }
            return new_author;
        }
        

        public List<Musician> SongtoAuthors(Song a)
        {
            var new_authors = new List<Musician> { a.Main_Author };
            if (a.extra_authors != null)
            {
                foreach (var musician in a.extra_authors)
                {
                    new_authors.Add(musician.Author);

                }

            }
            return new_authors;

        }



      
    }  
}