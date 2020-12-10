using System;
using System.Text;
using Endpointer.Authentication.API.Extensions;
using Endpointer.Authentication.API.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

[assembly: FunctionsStartup(typeof(MCLiveStatus.Authentication.Startup))]

namespace MCLiveStatus.Authentication
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IServiceCollection services = builder.Services;

            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration()
            {
                AccessTokenSecret = Environment.GetEnvironmentVariable("Authentication:AccessTokenSecret"),
                AccessTokenExpirationMinutes = 30,
                RefreshTokenSecret = Environment.GetEnvironmentVariable("Authentication:RefreshTokenSecret"),
                RefreshTokenExpirationMinutes = 131400,
                Audience = Environment.GetEnvironmentVariable("Authentication:Audience"),
                Issuer = Environment.GetEnvironmentVariable("Authentication:Issuer")
            };

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true
            };

            string connectionString = Environment.GetEnvironmentVariable("SqlServerConnectionString");
            services.AddEndpointerAuthentication(authenticationConfiguration,
                tokenValidationParameters,
                o => o.UseSqlServer(connectionString));
        }
    }
}
