using Microsoft.AspNetCore.WebUtilities;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MusicFree.Models;
using MusicFree.utilities;
using MongoDB.Bson;
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
            var filename = name.Split('.')[0];
            var file_type = name.Split('.')[1];


            await _gridfs.UploadFromStreamAsync(filename, fs,new GridFSUploadOptions{ Metadata=new BsonDocument { {"file_type",file_type }} });

        }



  


        private string ContentTypeReturn(GridFSFileInfo file)
        {
            Console.WriteLine(file.Metadata.GetValue("file_type").AsString);
            switch (file.Metadata.GetValue("file_type").AsString)
            {
                case "Signs of Love":
                    return "audio/wav";
                case "jpg":
                    return "image/jpeg";
                case "png":
                    return "image/png";

                case "wav":
                    return "audio/wav";
                case "mp3":
                    return "audio/mp3";

            }

            return "";

        }


        public async Task StreamSong(HttpContext context, string filename)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Where(x => x.Filename == filename);
            var result = _gridfs.Find(filter);
            result.MoveNext();
            var file = result.Current.ToList()[0];
            Console.WriteLine(ContentTypeReturn(file));
            context.Response.Headers.Append("Content-Type",ContentTypeReturn(file));
            context.Response.ContentLength = file.Length;
            await _gridfs.DownloadToStreamByNameAsync(filename, context.Response.Body);

        }
    }
}
