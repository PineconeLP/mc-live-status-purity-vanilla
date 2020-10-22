using System;
using Microsoft.EntityFrameworkCore;

namespace MCLiveStatus.EntityFramework.Contexts
{
    public class MCLiveStatusDbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public MCLiveStatusDbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public MCLiveStatusDbContext CreateDbContext()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();

            _configureDbContext(optionsBuilder);

            return new MCLiveStatusDbContext(optionsBuilder.Options);
        }
    }
}