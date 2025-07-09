using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MusicFree.Models;
namespace MusicFree
{
    public partial class UserContext
        : IdentityDbContext<User>
    {
    
       
        public DbSet<UserRadio> radios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


          
        }
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {
        }


    }
}
