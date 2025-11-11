using EF6TempTableKit.DbContext;
using MusicFree.Models.TempModels;
using System.Data.Entity;
namespace MusicFree
{
    [Configuration(typeof(EF6TempTableKitDbConfiguration))]
    public class TempContext : DbContext, IDbContextWithTempTable
    {


        public virtual DbSet<StackModel> stack { get; set; }

       

        public TempTableContainer TempTableContainer { get; set; } = new TempTableContainer();






    }
}
