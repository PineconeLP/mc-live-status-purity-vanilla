using System.Data.Common;
using MCLiveStatus.EntityFramework.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MCLiveStatus.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFTestsFixture
    {
        protected DbConnection _connection;
        protected MCLiveStatusDbContextFactory _contextFactory;

        [SetUp]
        public void BaseSetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            _contextFactory = new MCLiveStatusDbContextFactory(o => o.UseSqlite(_connection));
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }
        }

        [TearDown]
        public void BaseTearDown()
        {
            _connection.Dispose();
        }
    }
}