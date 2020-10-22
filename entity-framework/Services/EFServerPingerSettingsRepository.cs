using System;
using System.Threading.Tasks;
using AutoMapper;
using MCLiveStatus.Domain.Models;
using MCLiveStatus.Domain.Services;
using MCLiveStatus.EntityFramework.Contexts;
using MCLiveStatus.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace MCLiveStatus.EntityFramework.Services
{
    public class EFServerPingerSettingsRepository : IServerPingerSettingsRepository
    {
        private readonly MCLiveStatusDbContextFactory _contextFactory;
        private readonly IMapper _mapper;

        public EFServerPingerSettingsRepository(MCLiveStatusDbContextFactory contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<ServerPingerSettings> Load()
        {
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                ServerPingerSettingsDTO settingsDTO = await context.ServerPingerSettings.FirstOrDefaultAsync();

                if (settingsDTO == null)
                {
                    return null;
                }

                return _mapper.Map<ServerPingerSettings>(settingsDTO);
            }
        }

        public async Task<ServerPingerSettings> Save(ServerPingerSettings settings)
        {
            using (MCLiveStatusDbContext context = _contextFactory.CreateDbContext())
            {
                ServerPingerSettingsDTO settingsDTO = _mapper.Map<ServerPingerSettingsDTO>(settings);

                context.ServerPingerSettings.Update(settingsDTO);

                await context.SaveChangesAsync();

                return _mapper.Map<ServerPingerSettings>(settingsDTO);
            }
        }
    }
}