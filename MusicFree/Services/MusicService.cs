using MusicFree.Models;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
namespace MusicFree.Services { 

public class MusicService
{
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IGridFSBucket _gridfs;
    public MusicService()
        {

            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("MusicStore");
            _gridfs = new GridFSBucket(_database);
        }

        public async Task UploadFile(string name,Stream fs)
        {   
           
            await _gridfs.UploadFromStreamAsync(name,fs);
           
        }
        public  async Task StreamSong(HttpContext context, string filename)
        {
          await _gridfs.DownloadToStreamByNameAsync(filename, context.Response.Body );
        }
            
}
}