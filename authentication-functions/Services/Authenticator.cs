using System;
using System.Threading.Tasks;
using MCLiveStatus.Authentication.Exceptions;
using MCLiveStatus.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace MCLiveStatus.Authentication.Services
{
    public class Authenticator
    {
        private readonly UsersRepository _usersRepository;
        private readonly RefreshTokenRepository _refreshTokenRepository;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;

        public Authenticator(UsersRepository usersRepository,
            RefreshTokenRepository refreshTokenRepository,
            AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator)
        {
            _usersRepository = usersRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task Register(string email, string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                throw new PasswordsDoNotMatchException();
            }

            User userByEmail = await _usersRepository.GetByEmail(email);
            if (userByEmail != null)
            {
                throw new EmailExistsException(email);
            }

            User userByUsername = await _usersRepository.GetByUsername(username);
            if (userByUsername != null)
            {
                throw new UsernameExistsException(username);
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            User registrationUser = new User()
            {
                Email = email,
                Username = username,
                PasswordHash = passwordHash
            };

            await _usersRepository.Create(registrationUser);
        }

        public async Task<AuthenticatedUser> Login(string username, string password)
        {
            User existingUser = await _usersRepository.GetByUsername(username);
            if (existingUser == null)
            {
                throw new UsernameNotFoundException(username);
            }

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(password, existingUser.PasswordHash);
            if (!isCorrectPassword)
            {
                throw new InvalidPasswordException();
            }

            return await CreateAuthenticatedUser(existingUser);
        }

        public async Task<AuthenticatedUser> Refresh(string refreshToken)
        {
            RefreshToken storedRefreshToken = await _refreshTokenRepository.GetByToken(refreshToken);
            if (storedRefreshToken == null)
            {
                throw new InvalidRefreshTokenException();
            }

            await _refreshTokenRepository.Delete(storedRefreshToken.Id);

            User existingUser = await _usersRepository.GetById(storedRefreshToken.UserId);

            return await CreateAuthenticatedUser(existingUser);
        }

        public async Task Logout(Guid userId)
        {
            await _refreshTokenRepository.DeleteAll(userId);
        }

        private async Task<AuthenticatedUser> CreateAuthenticatedUser(User user)
        {
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(30);

            string accessToken = _accessTokenGenerator.GenerateToken(user, expirationTime);
            string refreshToken = _refreshTokenGenerator.GenerateToken(user.Id, DateTime.UtcNow.AddMonths(6));

            await _refreshTokenRepository.Create(new RefreshToken()
            {
                UserId = user.Id,
                Token = refreshToken
            });

            return new AuthenticatedUser()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpireTime = expirationTime
            };
        }
    }
}