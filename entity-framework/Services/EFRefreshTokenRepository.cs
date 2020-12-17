using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MCLiveStatus.Domain.Models;
using MCLiveStatus.Domain.Services;
using MCLiveStatus.EntityFramework.Contexts;
using MCLiveStatus.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace MCLiveStatus.EntityFramework.Services
{
    public class EFRefreshTokenRepository : IRefreshTokenRepository
    {
        public const int SINGLE_REFRESH_TOKEN_ID = 1;

        private readonly MCLiveStatusDbContextFactory _contextFactory;

        public EFRefreshTokenRepository(MCLiveStatusDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> GetRefreshToken()
        {
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                RefreshTokenDTO refreshToken = await context.RefreshTokens.FirstOrDefaultAsync(t => t.Id == SINGLE_REFRESH_TOKEN_ID);

                return refreshToken?.Token;
            }
        }

        public async Task SetRefreshToken(string refreshToken)
        {
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                RefreshTokenDTO refreshTokenDTO = new RefreshTokenDTO()
                {
                    Id = SINGLE_REFRESH_TOKEN_ID,
                    Token = refreshToken
                };

                bool exists = await context.RefreshTokens.AnyAsync(t => t.Id == refreshTokenDTO.Id);
                if (exists)
                {
                    context.RefreshTokens.Update(refreshTokenDTO);
                }
                else
                {
                    context.RefreshTokens.Add(refreshTokenDTO);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task ClearRefreshToken()
        {
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                RefreshTokenDTO refreshTokenDTO = new RefreshTokenDTO()
                {
                    Id = SINGLE_REFRESH_TOKEN_ID
                };

                bool exists = await context.RefreshTokens.AnyAsync(t => t.Id == refreshTokenDTO.Id);
                if (exists)
                {
                    context.RefreshTokens.Remove(refreshTokenDTO);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}