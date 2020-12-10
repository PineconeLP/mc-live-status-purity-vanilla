using System;
using System.Text;
using MCLiveStatus.Authentication.Contexts;
using MCLiveStatus.Authentication.Models;
using MCLiveStatus.Authentication.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

[assembly: FunctionsStartup(typeof(MCLiveStatus.Authentication.Startup))]

namespace MCLiveStatus.Authentication
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IServiceCollection services = builder.Services;

            string connectionString = Environment.GetEnvironmentVariable("SqlServerConnectionString");
            Action<DbContextOptionsBuilder> configureContext = o => o.UseSqlServer(connectionString);
            services.AddDbContext<AuthServerDbContext>(configureContext);
            services.AddSingleton<AuthServerDbContextFactory>(new AuthServerDbContextFactory(configureContext));

            builder.Services.AddOptions<TokenConfiguration>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("Authentication").Bind(settings);
                });

            services.AddSingleton(s =>
            {
                TokenConfiguration tokenConfiguration = s.GetRequiredService<IOptions<TokenConfiguration>>().Value;
                return new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguration.AccessTokenSecret)),
                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });

            services.AddSingleton<AccessTokenGenerator>();
            services.AddSingleton<AccessTokenDecoder>();
            services.AddSingleton<RefreshTokenGenerator>();
            services.AddSingleton<UsersRepository>();
            services.AddSingleton<RefreshTokenRepository>();
            services.AddSingleton<Authenticator>();
            services.AddSingleton<HttpRequestAuthenticator>();

            IServiceProvider provider = services.BuildServiceProvider();
            using (IServiceScope scope = provider.CreateScope())
            {
                AuthServerDbContextFactory contextFactory = scope.ServiceProvider.GetRequiredService<AuthServerDbContextFactory>();
                using (AuthServerDbContext context = contextFactory.CreateDbContext())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
