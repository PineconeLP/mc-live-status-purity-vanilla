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
    public class EFServerPingerSettingsRepositoryTests : EFTestsFixture
    {
        private EFServerPingerSettingsRepository _repository;

        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new MapperConfiguration(c => c.AddProfile<DTOMappingProfile>()).CreateMapper();

            _repository = new EFServerPingerSettingsRepository(_contextFactory, _mapper);
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
        public async Task Load_WithMultipleExistingSettings_ReturnsNewestSettings()
        {
            ServerPingerSettingsDTO settings1 = new ServerPingerSettingsDTO();
            ServerPingerSettingsDTO settings2 = new ServerPingerSettingsDTO();
            ServerPingerSettingsDTO settings3 = new ServerPingerSettingsDTO();
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                context.ServerPingerSettings.Add(settings1);
                context.ServerPingerSettings.Add(settings2);
                context.ServerPingerSettings.Add(settings3);
                await context.SaveChangesAsync();
            }

            ServerPingerSettings settings = await _repository.Load();

            Assert.AreEqual(settings.Id, settings3.Id);
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