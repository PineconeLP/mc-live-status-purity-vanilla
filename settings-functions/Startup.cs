using System;
using System.Text;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.TokenDecoders;
using MCLiveStatus.ServerSettings.Models;
using MCLiveStatus.ServerSettings.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

[assembly: FunctionsStartup(typeof(MCLiveStatus.ServerSettings.Startup))]

namespace MCLiveStatus.ServerSettings
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IServiceCollection services = builder.Services;

            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration()
            {
                AccessTokenSecret = Environment.GetEnvironmentVariable("Authentication_AccessTokenSecret"),
                Audience = Environment.GetEnvironmentVariable("Authentication_Audience"),
                Issuer = Environment.GetEnvironmentVariable("Authentication_Issuer")
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

            services.AddSingleton<IServerPingerSettingsRepository, FirebaseServerPingerSettingsRepository>();

            services.AddSingleton(tokenValidationParameters);
            services.AddSingleton<AccessTokenDecoder>();
            services.AddSingleton<HttpRequestAuthenticator>();
        }
    }
}
