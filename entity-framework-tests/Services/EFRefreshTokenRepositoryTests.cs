using System.Threading.Tasks;
using MCLiveStatus.EntityFramework.Contexts;
using MCLiveStatus.EntityFramework.Models;
using MCLiveStatus.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace MCLiveStatus.EntityFramework.Tests.Services
{
    [TestFixture]
    public class EFRefreshTokenRepositoryTests : EFTestsFixture
    {
        private EFRefreshTokenRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new EFRefreshTokenRepository(_contextFactory);
        }

        [Test]
        public async Task GetRefreshToken_WithNonExistingRefreshToken_ReturnsNull()
        {
            string refreshToken = await _repository.GetRefreshToken();

            Assert.IsNull(refreshToken);
        }

        [Test]
        public async Task GetRefreshToken_WithExistingRefreshToken_ReturnsRefreshToken()
        {
            string expectedRefreshToken = "123";
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                context.RefreshTokens.Add(new RefreshTokenDTO()
                {
                    Id = EFRefreshTokenRepository.SINGLE_REFRESH_TOKEN_ID,
                    Token = expectedRefreshToken
                });
                await context.SaveChangesAsync();
            }

            string refreshToken = await _repository.GetRefreshToken();

            Assert.IsNotNull(refreshToken);
            Assert.AreEqual(expectedRefreshToken, refreshToken);
        }

        [Test]
        public async Task SetRefreshToken_WithExistingRefreshToken_SetsNewRefreshToken()
        {
            string oldRefreshToken = "123";
            string newRefreshToken = "456";
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                context.RefreshTokens.Add(new RefreshTokenDTO()
                {
                    Id = EFRefreshTokenRepository.SINGLE_REFRESH_TOKEN_ID,
                    Token = oldRefreshToken
                });
                await context.SaveChangesAsync();
            }

            await _repository.SetRefreshToken(newRefreshToken);

            string currentRefreshToken = await _repository.GetRefreshToken();
            Assert.AreNotEqual(oldRefreshToken, currentRefreshToken);
            Assert.AreEqual(newRefreshToken, currentRefreshToken);
        }

        [Test]
        public async Task SetRefreshToken_WithNonExistingRefreshToken_SetsRefreshToken()
        {
            string expected = "456";

            await _repository.SetRefreshToken(expected);

            string currentRefreshToken = await _repository.GetRefreshToken();
            Assert.AreEqual(expected, currentRefreshToken);
        }

        [Test]
        public async Task ClearRefreshToken_WithExistingRefreshToken_ClearsRefreshToken()
        {
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                context.RefreshTokens.Add(new RefreshTokenDTO()
                {
                    Id = EFRefreshTokenRepository.SINGLE_REFRESH_TOKEN_ID,
                    Token = "123"
                });
                await context.SaveChangesAsync();
            }

            await _repository.ClearRefreshToken();

            string currentRefreshToken = await _repository.GetRefreshToken();
            Assert.IsNull(currentRefreshToken);
        }

        [Test]
        public async Task ClearRefreshToken_WithNonExistingRefreshToken_ClearsRefreshToken()
        {
            await _repository.ClearRefreshToken();

            string currentRefreshToken = await _repository.GetRefreshToken();
            Assert.IsNull(currentRefreshToken);
        }
    }
}