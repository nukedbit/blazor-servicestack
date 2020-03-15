using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.Host.NetCore;

namespace MyApp
{
 public class ServiceStackAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceStackAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {           
            return Task.FromResult(new AuthenticationState(Authenticate()));
        }

        public string RoleClaimType { get; set; } = ClaimTypes.Role;
        public string PermissionClaimType { get; set; } = "perm";

        public string Issuer { get; set; } = HostContext.ServiceName;

        public List<string> AdminRoles { get; set; } = new List<string> {
            RoleNames.Admin,
        };

        public string AuthenticationType { get; set; } = "Identity.Application";

        private ClaimsPrincipal Authenticate()
        {
            var req = new NetCoreRequest(this._httpContextAccessor.HttpContext, GetType().Name);
            var session = req.GetSession(reload: true);
           
            if (req.HttpContext.User?.Identity?.IsAuthenticated == true)
            {
                if (req.HttpContext.User.Identity is ClaimsIdentity identity &&
                    (HostContext.HasValidAuthSecret(req) || identity.HasClaim(RoleClaimType, RoleNames.Admin)))
                {
                    foreach (var adminRole in AdminRoles)
                    {
                        if (identity.HasClaim(RoleClaimType, adminRole))
                            continue;

                        identity.AddClaim(new Claim(RoleClaimType, adminRole, ClaimValueTypes.String, Issuer));
                    }

                    return new ClaimsPrincipal(identity);
                }
            }
            if (session.IsAuthenticated)
            {
                var claims = session.ConvertSessionToClaims(
                    issuer: Issuer,
                    roleClaimType: RoleClaimType,
                    permissionClaimType: PermissionClaimType);

                if (session.Roles.IsEmpty() && HostContext.AppHost.GetAuthRepository(req) is IManageRoles authRepo)
                {
                    using (authRepo as IDisposable)
                    {
                        var roles = authRepo.GetRoles(session.UserAuthId.ToInt());
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(RoleClaimType, role, Issuer));
                        }
                    }
                }

                if (HostContext.HasValidAuthSecret(req) || claims.Any(x => x.Type == RoleClaimType && x.Value == RoleNames.Admin))
                {
                    foreach (var adminRole in AdminRoles)
                    {
                        claims.Add(new Claim(RoleClaimType, adminRole, ClaimValueTypes.String, Issuer));
                    }
                }

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType));

                req.HttpContext.User = principal;
                return principal;
            }
            else if (HostContext.HasValidAuthSecret(req))
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, nameof(HostConfig.AdminAuthSecret), ClaimValueTypes.String, Issuer),
                    new Claim(ClaimTypes.Name, RoleNames.Admin, ClaimValueTypes.String, Issuer),
                    new Claim(ClaimTypes.GivenName, RoleNames.Admin, ClaimValueTypes.String, Issuer),
                    new Claim(ClaimTypes.Surname, "User", ClaimValueTypes.String, Issuer),
                };

                foreach (var adminRole in AdminRoles)
                {
                    claims.Add(new Claim(RoleClaimType, adminRole, ClaimValueTypes.String, Issuer));
                }

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType));

                req.HttpContext.User = principal;

                return principal;
            }
            return new ClaimsPrincipal();
        }

        public async Task RaiseLogout()
        {
            Authenticate();
            var task = Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
            NotifyAuthenticationStateChanged(task);
            await task;
        }
    }
}