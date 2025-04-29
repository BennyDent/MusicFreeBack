using MongoDB.Driver.GridFS;
using MongoDB.Driver;

namespace MusicFree.Services
{
    public class GridFsPlayer
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IGridFSBucket _gridfs;

        public GridFsPlayer() {
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("MusicStore");
            _gridfs = new GridFSBucket(_database);
        }

        public async Task UploadFile(string name, Stream fs)
        {

            await _gridfs.UploadFromStreamAsync(name, fs);

        }
        public async Task StreamSong(HttpContext context, string filename)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Where(x => x.Filename == filename);
            var result = _gridfs.Find(filter);
            result.MoveNext();
            var result_array = result.Current.ToList();

            Console.WriteLine(result_array);
            // context.Response.ContentLength = result_array[0].Length;
            await _gridfs.DownloadToStreamByNameAsync(filename, context.Response.Body);

        }
    }
}
