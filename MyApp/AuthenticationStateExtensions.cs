using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceStack;

namespace MyApp {
    public static class AuthenticationStateExtensions {
        public static JsonServiceClient ToClient(this AuthenticationState state){
            var navigationManager = HostContext.Container.Resolve<NavigationManager>();
            var baseUri = navigationManager.BaseUri;
            var sessionId = state.User.Claims.Where(c => c.Type == "SessionId").FirstOrDefault();
            var client = new JsonServiceClient(baseUri);
            client.Headers["X-ss-id"] = sessionId.Value;
            return client;
        }
    }
}
