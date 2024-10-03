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
    }
}
