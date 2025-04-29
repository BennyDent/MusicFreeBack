using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MusicFree.Models;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
namespace MusicFree
{
    public partial class UserContext
        : IdentityDbContext<User>
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
