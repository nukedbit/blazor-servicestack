using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace MyApp
{
    public static class ComponentExtensions
    {
        public static  IServiceStackClient GetServiceStackClient(this ComponentBase _, AuthenticationStateProvider provider)
        {
            return new ServiceStackClient(provider);
        }
    }
}
