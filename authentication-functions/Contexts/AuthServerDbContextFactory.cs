using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MCLiveStatus.Authentication.Contexts
{
    public class AuthServerDbContextFactory
    {
        private Action<DbContextOptionsBuilder> _configureContext;

        public AuthServerDbContextFactory(Action<DbContextOptionsBuilder> configureContext)
        {
            _configureContext = configureContext;
        }

        public AuthServerDbContext CreateDbContext()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();

            _configureContext(optionsBuilder);

            return new AuthServerDbContext(optionsBuilder.Options);
        }
    }
}