using System;
using System.Threading.Tasks;
using MCLiveStatus.Authentication.Contexts;
using MCLiveStatus.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace MCLiveStatus.Authentication.Services
{
    public class UsersRepository
    {
        private readonly AuthServerDbContextFactory _contextFactory;

        public UsersRepository(AuthServerDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User> GetById(Guid userId)
        {
            using (AuthServerDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Users.FindAsync(userId);
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            using (AuthServerDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
        }

        public async Task<User> GetByUsername(string username)
        {
            using (AuthServerDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
        }

        public async Task<User> Create(User user)
        {
            user.Id = Guid.NewGuid();

            using (AuthServerDbContext context = _contextFactory.CreateDbContext())
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();

                return user;
            }
        }
    }
}