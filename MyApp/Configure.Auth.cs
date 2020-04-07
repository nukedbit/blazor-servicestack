using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Data;
using ServiceStack.FluentValidation;
using ServiceStack.OrmLite;

namespace MyApp
{
    // Add any additional metadata properties you want to store in the Users Typed Session
    public class CustomUserSession : AuthUserSession
    {
    }

    // Custom Validator to add custom validators to built-in /register Service requiring DisplayName and ConfirmPassword
    public class CustomRegistrationValidator : RegistrationValidator
    {
        public CustomRegistrationValidator()
        {
            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.ConfirmPassword).NotEmpty();
            });
        }
    }

    public class ConfigureAuth : IConfigureAppHost, IConfigureServices
    {
        public void Configure(IServiceCollection services)
        {

        }

        public void Configure(IAppHost appHost)
        {
            var AppSettings = appHost.AppSettings;
            appHost.Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                new IAuthProvider[] {
                    new ApiKeyAuthProvider(AppSettings)
                        {
                            KeyTypes = new[] {"secret", "publishable"},
                        },
                    new NetCoreIdentityAuthProvider(AppSettings) { // Adapter to enable ServiceStack Auth in MVC
                        AdminRoles = { "Manager" }, // Automatically Assign additional roles to Admin Users
                          CreateClaimsPrincipal = (claims, session, req) =>
                            {
                                var apiRepo = (IManageApiKeys)HostContext.TryResolve<IAuthRepository>();
                                var apiKeys = apiRepo.GetUserApiKeys(session.UserAuthId);
                                var apiKey = apiKeys.First(k => k.KeyType == "publishable" && k.Environment == "live");
                                claims.Add(new Claim("ApiKey", apiKey.Id ));
                                var identity = new ClaimsIdentity(claims, "Identity.Application");
                                return new ClaimsPrincipal(identity);
                            }
                    },
                    new CredentialsAuthProvider(AppSettings), // Sign In with Username / Password credentials 
                    new FacebookAuthProvider(AppSettings),    // Create App at: https://developers.facebook.com/apps
                    new TwitterAuthProvider(AppSettings),     // Create App at: https://dev.twitter.com/apps
                    new GoogleAuthProvider(AppSettings),      // https://console.developers.google.com/apis/credentials
                    new MicrosoftGraphAuthProvider(AppSettings), // Create App https://apps.dev.microsoft.com
                }));

            appHost.Plugins.Add(new RegistrationFeature()); //Enable /register Service

            //override the default registration validation with your own custom implementation
            appHost.RegisterAs<CustomRegistrationValidator, IValidator<Register>>();


            appHost.AfterInitCallbacks.Add(host =>
            {
                try
                {
                    var authProvider = (ApiKeyAuthProvider)AuthenticateService.GetAuthProviders()
                        .First(x => x is ApiKeyAuthProvider);
                    using (var db = host.TryResolve<IDbConnectionFactory>().Open())
                    {
                        var userWithKeysIds = db.Column<string>(db.From<ApiKey>()
                            .SelectDistinct(x => x.UserAuthId)).Map(int.Parse);

                        var userIdsMissingKeys = db.Column<string>(db.From<AppUser>()
                            .Where(x => userWithKeysIds.Count == 0 || !userWithKeysIds.Contains(x.Id))
                            .Select(x => x.Id));

                        var authRepo = (IManageApiKeys)host.TryResolve<IAuthRepository>();
                        foreach (var userId in userIdsMissingKeys)
                        {
                            var apiKeys = authProvider.GenerateNewApiKeys(userId.ToString());
                            authRepo.StoreAll(apiKeys);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }
    }
}
