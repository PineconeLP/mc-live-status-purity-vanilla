using Endpointer.Authentication.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MCLiveStatus.Authentication.Functions.Contexts
{
    public class AuthenticationDbContextDesignFactory : IDesignTimeDbContextFactory<AuthenticationDbContext>
    {
        public AuthenticationDbContext CreateDbContext(string[] args)
        {
            string connectionString = "Server=localhost,1433;Initial Catalog=LiveStatus;User ID=sa;Password=iLovePineconeLP*123;";
            DbContextOptions<AuthenticationDbContext> options = new DbContextOptionsBuilder<AuthenticationDbContext>()
                .UseSqlServer(connectionString, o => o.MigrationsAssembly("MCLiveStatus.Authentication.Functions"))
                .Options;

            return new AuthenticationDbContext(options);
        }
    }
}