using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyApp
{
    public static class ComponentExtensions
    {
        public static  IServiceStackClient GetServiceStackClient(this ComponentBase _, AuthenticationState state)
        {
            return new ServiceStackClient(state);
        }
    }
}
