using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCLiveStatus.Authentication.Contexts;
using MCLiveStatus.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace MCLiveStatus.Authentication.Services
{
    public class RefreshTokenRepository
    {
        private readonly AuthServerDbContextFactory _contextFactory;

        public RefreshTokenRepository(AuthServerDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<RefreshToken> GetByToken(string refreshToken)
        {
            using (AuthServerDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
            }
        }

        public async Task<RefreshToken> Create(RefreshToken refreshToken)
        {
            refreshToken.Id = Guid.NewGuid();

            using (AuthServerDbContext context = _contextFactory.CreateDbContext())
            {
                context.RefreshTokens.Add(refreshToken);
                await context.SaveChangesAsync();

                return refreshToken;
            }
        }

        public async Task Delete(Guid id)
        {
            using (AuthServerDbContext context = _contextFactory.CreateDbContext())
            {
                RefreshToken refreshToken = await context.RefreshTokens.FindAsync(id);
                if (refreshToken != null)
                {
                    context.RefreshTokens.Remove(refreshToken);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteAll(Guid userId)
        {
            using (AuthServerDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<RefreshToken> refreshTokens = await context.RefreshTokens
                    .Where(t => t.UserId == userId)
                    .ToListAsync();

                context.RefreshTokens.RemoveRange(refreshTokens);
                await context.SaveChangesAsync();
            }
        }
    }
}