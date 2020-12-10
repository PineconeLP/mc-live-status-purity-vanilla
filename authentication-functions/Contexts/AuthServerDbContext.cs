using MCLiveStatus.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace MCLiveStatus.Authentication.Contexts
{
    public class AuthServerDbContext : DbContext
    {
        public AuthServerDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}