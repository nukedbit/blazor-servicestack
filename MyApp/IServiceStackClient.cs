using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceStack;
using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp
{
    public interface IServiceStackClient
    {
        public Task DeleteAsync(IReturnVoid requestDto, CancellationToken token = default);
    }

    public class ServiceStackClient : IServiceStackClient
    {
        private AuthenticationStateProvider provider;

        public ServiceStackClient(AuthenticationStateProvider provider)
        {
            this.provider = provider;
        }

        private static IServiceGatewayAsync GetServiceGateway(IRequest request)
        {
            if (request is null)
            {
                return HostContext.AppHost.GetServiceGateway() as IServiceGatewayAsync;
            }
            return HostContext.AppHost.GetServiceGateway(request) as IServiceGatewayAsync;
        }

        private async Task<IRequest> GetRequestWithStateAsync(string verb = "GET")
        {
            var state = await provider.GetAuthenticationStateAsync();

            var httpRequest = new BlazorFakeHttpRequest(new BlazorFakeHttpContext(state.User));
            return new BlazorRequest(httpRequest, verb);
        }

        public async Task DeleteAsync(IReturnVoid requestDto, CancellationToken token = default)
        {
            var request = await GetRequestWithStateAsync("DELETE");
            var gateway = GetServiceGateway(request); 


            await HostContext.AppHost.ApplyPreAuthenticateFiltersAsync(request, request.Response);
            await HostContext.ApplyRequestFiltersAsync(request, request.Response, requestDto);
            await gateway.SendAsync<string>(requestDto);
        }

    }

    public static class ComponentExtensions2
    {
        public static  IServiceStackClient GetServiceStackClient(this ComponentBase _, AuthenticationStateProvider provider)
        {
            return new ServiceStackClient(provider);
        }
    }
}
