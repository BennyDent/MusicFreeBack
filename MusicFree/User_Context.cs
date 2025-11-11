using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MusicFree.Models;
namespace MusicFree
{
    public partial class UserContext
        : IdentityDbContext
    {
    
    
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
