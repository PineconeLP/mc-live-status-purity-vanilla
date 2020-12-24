using MCLiveStatus.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace MCLiveStatus.EntityFramework.Contexts
{
    public class MCLiveStatusDbContext : DbContext
    {
        public MCLiveStatusDbContext(DbContextOptions options) : base(options) { }

        public DbSet<ServerPingerSettingsDTO> ServerPingerSettings { get; set; }
        public DbSet<RefreshTokenDTO> RefreshTokens { get; set; }
    }
}