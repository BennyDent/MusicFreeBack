using MusicFree.Models;
namespace MusicFree.Graphql
{
    public class Query
    {

        [UseProjection]
        [UseFiltering]
        public IQueryable<Musician> GetAuthor([Service] FreeMusicContext dbContext)
        {
            return dbContext.musicians;
        }

        [UseProjection]
        [UseFiltering]
        public IQueryable<Albumn> GetAlbumn([Service] FreeMusicContext dbContext)
        {
            return dbContext.albumns;
        }
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Song> GetSong([Service] FreeMusicContext dbContext)
        {
            return dbContext.songs;
        }
        [UseProjection]
        [UseFiltering]
        public IQueryable<SongAuthor> GetSongAuthor([Service] FreeMusicContext dbContext)
        {
            return dbContext.song_authors;
        }
        [UseProjection]
        [UseFiltering]
        public IQueryable<AlbumnAuthor> GetAlbumnAuthor([Service] FreeMusicContext dbContext)
        {
            return dbContext.albumn_authors;
        }
        [UseProjection]
        [UseFiltering]
        public IQueryable<SongViews> GetSongViews([Service] FreeMusicContext dbContext)
        {
            return dbContext.songsViews;
        }

    }
}
