using System;
using System.Text;
using System.Threading.Tasks;
using Endpointer.Authentication.API.Services.Authenticators;
using Endpointer.Authentication.API.Services.TokenDecoders;
using Firebase.Database;
using Google.Apis.Auth.OAuth2;
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
        private readonly string _environment;

        private bool IsDevelopment => _environment == "Development";

        public Startup()
        {
            _environment = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
        }

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

            services.AddSingleton(tokenValidationParameters);
            services.AddSingleton<AccessTokenDecoder>();
            services.AddSingleton<HttpRequestAuthenticator>();

            FirebaseClient firebaseClient = CreateFirebaseClient();
            services.AddSingleton(firebaseClient);

            services.AddSingleton<IServerPingerSettingsRepository, FirebaseServerPingerSettingsRepository>();
        }

        private FirebaseClient CreateFirebaseClient()
        {
            return new FirebaseClient("https://mclivestatus-default-rtdb.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = async () =>
                    {
                        GoogleCredential credential = GetGoogleCredential();

                        return await credential.CreateScoped(
                                "https://www.googleapis.com/auth/userinfo.email",
                                "https://www.googleapis.com/auth/firebase.database")
                            .UnderlyingCredential.GetAccessTokenForRequestAsync();
                    },
                    AsAccessToken = true
                });
        }

        private GoogleCredential GetGoogleCredential()
        {
            if (IsDevelopment)
            {
                return GoogleCredential.FromFile("./firebase-credential.json");
            }
            else
            {
                return GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_CONFIG"));
            }
        }
    }
}
