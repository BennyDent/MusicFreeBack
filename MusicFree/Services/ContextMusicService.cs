
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MusicFree.Models;
using MusicFree.Models.DataReturnModel;
using MusicFree.Models.ExtraModels;
using MusicFree.Models.GenreAndName;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MusicFree.Services
{
    public class ContextMusicService
    {
        private readonly UserContext _user_context;
        private readonly FreeMusicContext _context;
        private readonly UserManager<User> _userManager;
        public readonly int strict_compare;
        public readonly int loose_compare;
        public ContextMusicService(UserManager<User> userManager, FreeMusicContext context, UserContext usercontext)
        {
            _user_context = usercontext;
            _userManager = userManager;
            _context = context;
            strict_compare = 5;
            loose_compare = 3;
        }


        

        public Boolean GenreSimilar(Genre genre, Genre compare)
        {
            var is_similar = false;

            var genre_similar = new List<string>();
            foreach (var value in genre.similar)
            {
                genre_similar.Add((genre.Name == value.genres.First().Name) ? value.genres.First().Name : value.genres.Last().Name);
            }

            if (genre_similar.Contains(compare.Name))
            {
                return true;
            }
            return false;

        }

 
        public Boolean ListGenreSimilar(List<Genre> author_genres, List<Genre> main_author_genres, int compare_index)
        {

            var counter = compare_index-2;


            foreach (var main_author_genre in main_author_genres)
            {

                foreach (var author_genre in author_genres)
                {
                    if (GenreSimilar(author_genre, main_author_genre))
                    {
                        if (counter != 0)
                        {
                            counter--;
                        }
                        else
                        {
                            return true;
                        }

                    }

                }

            }
            return false;
        }

    

        public Boolean ListTagSimilar(List<Tags> author_genres, List<Tags> main_author_genres, int compare_index)
        {

            var counter = compare_index-2;


            foreach (var main_author_genre in main_author_genres)
            {

                foreach (var author_genre in author_genres)
                {
                    if (TagSimilar(author_genre, main_author_genre))
                    {
                        if (counter != 0)
                        {
                            counter--;
                        }
                        else
                        {
                            return true;
                        }

                    }

                }

            }
            return false;
        }



        public List<Genre> CollectionAlbumntoGenres(List<Albumn> albumns)
        {
            var genres = new List<Genre>(); 
            foreach (var albumn in albumns)
            {

                genres = genres.Concat(AlbumnToGenre(albumn)).ToList();
            }
            return genres;
        }

        public List<Tags> CollectionAlbumntoTags(List<Albumn> albumns)
        {
            var genres = new List<Tags>();
            foreach (var albumn in albumns)
            {

                genres = genres.Concat(AlbumnToTag(albumn)).ToList();
            }
            return genres;
        }


        public List<Genre> MusicianToGenre(Musician author) {

            var result = new List<Genre>();
            foreach (var genre in author.genres)
            {

                result.Add(genre.genre);
            }
            return result;
        }


        public Boolean TagSimilar(Tags tag, Tags compare)
        {
            var is_similar = false;

            var genre_similar = new List<string>();
            foreach (var value in tag.similar)
            {
                genre_similar.Add((tag.Name == value.tags.First().Name) ? value.tags.First().Name : value.tags.Last().Name);
            }

            if (genre_similar.Contains(compare.Name))
            {
                return true;
            }
            return false;
        }


        public List<SongReturn> SongstoSongReturns(List<Song> songs, User user)
        {

           

            var songs_return = new List<SongReturn>();

            foreach (Song song in songs)
            {
                songs_return.Add(new SongReturn(song,(user!=null)? isSongLiked(song, user): null ));
            }
            return songs_return;
        }


        public List<Tags> MusicianTags(Musician author)
        {
            return CollectionAlbumntoTags(AuthorMainAlbumns(author, false));
        }

public List<Genre> AlbumnToGenre(Albumn a)
        {
            var b = a.genres;
            var genres = new List<Genre>();

            foreach (var genre in b)
            {


                genres.Add(genre.genre);
            }
            return genres;
        }

        public List<Tags> AlbumnToTag(Albumn a)
        {

            var b = a.tags;
            var tags = new List<Tags>();
            foreach (var tag in b)
            {


                tags.Add(tag.tag);
            }
            return tags;
        }



    
        



        public Boolean GenreCompare(List<Genre> input_genre, List<Genre> compare_genre, Boolean is_similar, int compare_index)
        {
            var is_genre_similar = ListGenreSimilar(input_genre, compare_genre, compare_index);

            if (input_genre.Intersect(compare_genre).Count() > compare_index | (is_similar && is_genre_similar))
            {
                return true;
            }
            else return false;
        }


        private List<Genre> MusicianAlbumnsToGenre(Musician main_author)
            {
                var a = main_author.Albumns;
                var new_list  = new List<Genre>();
                foreach(var albumn in a)
                {
                    new_list = new_list.Concat(AlbumnToGenre(albumn)).Distinct().ToList();
                }

                return new_list;
            }

        private    List<Tags> MusicianAlbumnsToTag(Musician main_author) {

                var a = main_author.Albumns;
                var new_list = new List<Tags>();
                foreach (var albumn in a)
                {
                    new_list = new_list.Concat(AlbumnToTag(albumn)).Distinct().ToList();
                }
                return new_list;
            }



        public List<Musician> GetSimilarAuthors(Musician author, int take_index) { 
        
        var musicians = new List<Musician>();

          

        int SongListenedbyOthersUsers(Musician a) {

               return a.listened_by.Where(a => author.listened_by.Where(b => b.UserId == a.UserId).Any()).ToList().Count();


            
            
            }


        
                return _context.musicians.Where(a=>GenreCompare(MusicianAlbumnsToGenre(a), MusicianAlbumnsToGenre(author),false, strict_compare)&&
                 TagCompare(MusicianAlbumnsToTag(a), MusicianAlbumnsToTag(author), false, strict_compare)).OrderBy(SongListenedbyOthersUsers).Take(take_index).ToList();   
            



        }


        public Boolean TagCompare(List<Tags> input_tag, List<Tags> compare_tag, Boolean is_similar, int compare_index)
        {
            

            var is_tag_similar = ListTagSimilar(input_tag, compare_tag, compare_index);

            if (input_tag.Intersect(compare_tag).Count() > compare_index-1 | (is_similar && is_tag_similar))
            {
                return true;
            }
            else return false;
        }

        public List<Albumn> AuthorMainAlbumns(Musician author, bool is_albumns)
        {
          int Listened(Albumn a)
            {
                return a.albumn_views.Count;
            }
            int Liked(Albumn a)
            {

                return a.liked_by.Count;
            }

            if (is_albumns) {
                return author.Albumns.Where(a=> a.albumn_type==AlbumnType.albumn).OrderBy(Listened).ThenBy(Liked).Take(5).ToList();
            }
            else
            {
                return author.Albumns.OrderBy(Listened).ThenBy(Liked).Take(5).ToList();
            }
            
        }



        public ICollection<Tags> AuthorTags(Musician author)
        {
            return CollectionAlbumntoTags(AuthorMainAlbumns(author, false));
        }



        public bool isSongListened(Song song, string id)
        {
           
            if (song.song_views.Where(a => a.UserId == id).Any()) {

                return true;
            }
            return false;

        }


        public Boolean isSongListened(Song song, User user)
        {
            if (user.song_views.Contains(song.Id) && song.song_views.Where(a => a.UserId == user.Id).Any())
            {
                return true;
            }
            else
            {
                if (user.song_views.Contains(song.Id))
                {
                    _context.songsViews.Add(new SongViews(user.Id, song.Id, song));
                     _context.SaveChanges();
                    return true;
                    
                   
                }
                if (song.song_views.Where(a => a.UserId == user.Id).Any())
                {
                    user.song_views.Add(song.Id);
                   _userManager.UpdateAsync(user);

                    return true;
                }
                return false;
            }
          
        }




        public DateTime SongLastListened(Song song, User user) {

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







        private SongViews? SongView(Song song, User user)
        {
            try { 
            return song.song_views.Where(a => a.UserId == user.Id).First();
            }
            catch {  return null; } 
        }

        public Boolean isSongLiked(Song song, User user)
        {
            if (user.song_likes.Contains(song.Id) && song.liked_by.Where(a => a.UserId == user.Id).Any())
            {
                return true;
            }
            else
            {
                if (user.song_likes.Contains(song.Id))
                {
                   _context.likes.Add(new UserSong(song.Id,user.Id, song));
                    _context.SaveChanges();
                    return true;
                }
                if (song.liked_by.Where(a => a.UserId == user.Id).Any())
                {
                   
                       user.song_likes.Add(song.Id);
                    _userManager.UpdateAsync(user);

                    return true;
                    
                }
                return false;
            }
        }

      
    }
}
