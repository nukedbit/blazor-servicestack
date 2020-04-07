using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceStack;

namespace MyApp {
    public static class AuthenticationStateExtensions {
        public static JsonServiceClient ToClient(this AuthenticationState state){
            var navigationManager = HostContext.Container.Resolve<NavigationManager>();
            var baseUri = navigationManager.BaseUri;var apiKey = state.User.Claims.Where(c => c.Type == "ApiKey").FirstOrDefault();
            var client = new JsonServiceClient(navigationManager.BaseUri);
            client.BearerToken = apiKey?.Value;
            return client;
        }
    }
}