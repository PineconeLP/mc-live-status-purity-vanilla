using System;
using System.Text;
using Endpointer.Authentication.API.Extensions;
using Endpointer.Authentication.API.Firebase.Extensions;
using Endpointer.Authentication.API.Models;
using Firebase.Database;
using Google.Apis.Auth.OAuth2;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

[assembly: FunctionsStartup(typeof(MCLiveStatus.Authentication.Startup))]

namespace MCLiveStatus.Authentication
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
                AccessTokenExpirationMinutes = 30,
                RefreshTokenSecret = Environment.GetEnvironmentVariable("Authentication_RefreshTokenSecret"),
                RefreshTokenExpirationMinutes = 131400,
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

            FirebaseClient firebaseClient = CreateFirebaseClient();
            services.AddEndpointerAuthentication(authenticationConfiguration,
                tokenValidationParameters,
                o => o.WithFirebaseDataSource(firebaseClient));
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
