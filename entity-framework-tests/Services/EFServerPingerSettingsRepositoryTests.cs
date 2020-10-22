using System;
using System.Data.Common;
using System.Threading.Tasks;
using AutoMapper;
using MCLiveStatus.Domain.Models;
using MCLiveStatus.EntityFramework.Contexts;
using MCLiveStatus.EntityFramework.Mappers;
using MCLiveStatus.EntityFramework.Models;
using MCLiveStatus.EntityFramework.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace MCLiveStatus.EntityFramework.Tests.Services
{
    /// <summary>
    /// Kind of an integration test.
    /// </summary>
    [TestFixture]
    public class EFServerPingerSettingsRepositoryTests
    {
        private EFServerPingerSettingsRepository _repository;

        private MCLiveStatusDbContextFactory _contextFactory;
        private DbConnection _connection;
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
            _contextFactory = new MCLiveStatusDbContextFactory(o => o.UseSqlite(_connection));
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                context.Database.Migrate();
            }
            _mapper = new MapperConfiguration(c => c.AddProfile<DTOMappingProfile>()).CreateMapper();

            _repository = new EFServerPingerSettingsRepository(_contextFactory, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _connection.Dispose();
        }

        [Test]
        public async Task Load_WithExistingSettings_ReturnsSettings()
        {
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                context.ServerPingerSettings.Add(new ServerPingerSettingsDTO());
                await context.SaveChangesAsync();
            }

            ServerPingerSettings settings = await _repository.Load();

            Assert.IsNotNull(settings);
        }

        [Test]
        public async Task Load_WithNonExistingSettings_ReturnsNull()
        {
            ServerPingerSettings settings = await _repository.Load();

            Assert.IsNull(settings);
        }

        [Test]
        public async Task Load_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            try
            {
                await _repository.Load();
                Assert.Fail();
            }
            catch (System.Exception) { }
        }

        [Test]
        public async Task Save_WithExistingSettings_UpdatesSettings()
        {
            double expectedPingIntervalSeconds = 100;
            int id = 1;
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                context.ServerPingerSettings.Add(new ServerPingerSettingsDTO() { Id = id });
                await context.SaveChangesAsync();
            }

            await _repository.Save(new ServerPingerSettings(id) { PingIntervalSeconds = expectedPingIntervalSeconds });

            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                ServerPingerSettingsDTO settings = await context.ServerPingerSettings.FirstOrDefaultAsync();
                double actualPingIntervalSeconds = settings.PingIntervalSeconds;
                Assert.AreEqual(expectedPingIntervalSeconds, actualPingIntervalSeconds);
            }
        }

        [Test]
        public async Task Save_WithNonExistingSettings_CreatesSettings()
        {
            double expectedPingIntervalSeconds = 100;

            await _repository.Save(new ServerPingerSettings() { PingIntervalSeconds = expectedPingIntervalSeconds });

            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                ServerPingerSettingsDTO settings = await context.ServerPingerSettings.FirstOrDefaultAsync();
                double actualPingIntervalSeconds = settings.PingIntervalSeconds;
                Assert.AreEqual(expectedPingIntervalSeconds, actualPingIntervalSeconds);
            }
        }

        [Test]
        public async Task Save_WithNonExistingSettings_ReturnsSettingsWithId()
        {
            double expectedPingIntervalSeconds = 100;

            ServerPingerSettings settings = await _repository.Save(new ServerPingerSettings() { PingIntervalSeconds = expectedPingIntervalSeconds });

            Assert.AreNotEqual(settings.Id, 0);
        }

        [Test]
        public async Task Save_WithFailure_ThrowsException()
        {
            _connection.Dispose();

            try
            {
                await _repository.Save(new ServerPingerSettings());
                Assert.Fail();
            }
            catch (Exception) { }
        }
    }
}