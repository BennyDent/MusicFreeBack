using System.Data.Entity;

namespace MusicFree
{
    public class Configuration: DbConfigurationTypeAttribute
    {


        public Configuration(Type type) : base(type)
        {
        }
    }
}
