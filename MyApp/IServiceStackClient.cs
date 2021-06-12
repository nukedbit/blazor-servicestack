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
        public Task PostAsync(IReturnVoid requestDto, CancellationToken token = default);
        public Task PutAsync(IReturnVoid requestDto, CancellationToken token = default);
        public Task DeleteAsync(IReturnVoid requestDto, CancellationToken token = default);


        public Task<TResponse> GetAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default);
        public Task<TResponse> PostAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default);
        public Task<TResponse> PutAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default);
        public Task<TResponse> DeleteAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default);
    }

    public partial class ServiceStackClient : IServiceStackClient
    {
        private AuthenticationState state;

        public ServiceStackClient(AuthenticationState state)
        {
            this.state = state;
        }

        private static IServiceGatewayAsync GetServiceGateway(IRequest request)
        {
            if (request is null)
            {
                return HostContext.AppHost.GetServiceGateway() as IServiceGatewayAsync;
            }
            return HostContext.AppHost.GetServiceGateway(request) as IServiceGatewayAsync;
        }

        private IRequest GetRequestWithState(string verb = "GET")
        {
            var httpRequest = new BlazorFakeHttpRequest(new BlazorFakeHttpContext(state.User));
            return new BlazorRequest(httpRequest, verb);
        }

        public async Task DeleteAsync(IReturnVoid requestDto, CancellationToken token = default)
        {
            await SendAsync(requestDto, "DELETE", token);
        }

        public async Task PutAsync(IReturnVoid requestDto, CancellationToken token = default)
        {
            await SendAsync(requestDto, "PUT", token);
        }

        public async Task PostAsync(IReturnVoid requestDto, CancellationToken token = default)
        {
            await SendAsync(requestDto, "POST", token);
        }

        private async Task SendAsync(IReturnVoid requestDto, string verb, CancellationToken token = default)
        {
            var request = GetRequestWithState(verb);
            var gateway = GetServiceGateway(request);
            await HostContext.AppHost.ApplyPreAuthenticateFiltersAsync(request, request.Response);
            await HostContext.ApplyRequestFiltersAsync(request, request.Response, requestDto);
            await gateway.SendAsync<string>(requestDto, token);
        }

        private async Task<TResponse> SendAsync<TResponse>(IReturn<TResponse> requestDto, string verb, CancellationToken token = default)
        {
            var request = GetRequestWithState(verb);
            var gateway = GetServiceGateway(request);
            await HostContext.AppHost.ApplyPreAuthenticateFiltersAsync(request, request.Response);
            await HostContext.ApplyRequestFiltersAsync(request, request.Response, requestDto);
            return await gateway.SendAsync<TResponse>(requestDto, token);
        }

        public async Task<TResponse> GetAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default)
        {
            return await SendAsync(requestDto, "GET", token);
        }

        public async Task<TResponse> PostAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default)
        {
            return await SendAsync(requestDto, "POST", token);
        }

        public async Task<TResponse> PutAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default)
        {
            return await SendAsync(requestDto, "PUT", token);
        }

        public async Task<TResponse> DeleteAsync<TResponse>(IReturn<TResponse> requestDto, CancellationToken token = default)
        {
            return await SendAsync(requestDto, "DELETE", token);
        }
    }
}
