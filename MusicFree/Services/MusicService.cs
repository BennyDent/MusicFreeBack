using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MusicFree.Models;
using MusicFree.Models.DataReturnModel;
using MusicFree.Models.GenreAndName;
namespace MusicFree.Services { 

public class MusicService
{
       
       
    public MusicService()
        {
            
            
        }

       

     


        public List<AlbumnReturn> AlbumnstoAlbumnReturn(List<Albumn> albumns)
        {
            var albumns_return = new List<AlbumnReturn>();
            foreach(var albumn in albumns)
            {

                albumns_return.Add(new AlbumnReturn(albumn));
            }

            return albumns_return;
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

        

       


       



        public SongViews? SongView(Song song, User user)
        {
            try
            {
                return song.song_views.Where(a => a.UserId == user.Id).First();
            }
            catch { return null; }
        }


        public AlbumnViews? GetAlbumnView(Albumn albumn, User user) {
            try {

                return albumn.albumn_views.Where(a=> a.UserId==user.Id).First();
            } catch { return null; }
        
        
        }

        
        public List<object> AutoIncrementParentReturn(List<AutoIncrementedParent>input )
        {
            var list = new List<object>();

            foreach(var part in input)
            {
                switch (part.GetType().Name)
                {
                 case "Song":
                    list.Add(new SongReturn((Song)part, false));
                    break;
                case "Musician":

                    list.Add(new AuthorReturn((Musician)part));
                    break;

                    case "Albumn":
                        list.Add(new AlbumnReturn((Albumn)part));
                        break;
                }
              


                }


            return list;
        }
        public List<object> SeveralClassReturn<T>(List<T>input)
        {
            var list = new List<object>();
            foreach(var part in input)
            {
                switch (part.GetType().Name)
                {
                    case "Song":
                        list.Add(new SongReturn((Song)(object)part,false));
                        break;
                    case "Genre":
                        list.Add(new { Id = ((Genre)(object)part).Name });
                        break;
                    case "Tag":
                        list.Add(new { Id = ((Tag)(object)part).Name });
                        break;
                    case "Musician":
                        
                        list.Add(new AuthorReturn((Musician)(object)part));
                            break;
                }
            }
            return list;
        }



      

        public DateTime SongLastListened(Song song, User user)
        {

            if (SongView(song, user) == null)
            {
                throw new Exception();
            }
            else
            {

                return SongView(song, user).last_listened;
            }

        }


      



        public int SongListened(Song song, User user)
        {
            if (SongView(song, user) == null)
            {
                return 0;
            }
            else
            {
                return SongView(song, user).listened;
            }
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