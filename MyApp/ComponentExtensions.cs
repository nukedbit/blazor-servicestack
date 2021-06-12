using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ServiceStack;
using ServiceStack.Web;

namespace MyApp
{
    public static class ComponentExtensions {
        public static IServiceGatewayAsync GetServiceGateway(this IComponent cmp, IRequest request = null){
            if(request is null)
            {
                return HostContext.AppHost.GetServiceGateway() as IServiceGatewayAsync;
            }
            return HostContext.AppHost.GetServiceGateway(request) as IServiceGatewayAsync; 
        }

        public static async Task<IRequest> GetRequestAsync(this IComponent cmp, AuthenticationStateProvider provider)
        {
            var state = await provider.GetAuthenticationStateAsync();

            var httpRequest = new BlazorFakeHttpRequest(new BlazorFakeHttpContext(state.User));
            return new BlazorRequest(httpRequest);
        }

        public static async Task<TResponse> SendAsync<TResponse>(this IComponent cmp, AuthenticationStateProvider provider, IReturn<TResponse> requestDto, CancellationToken token = default)
        {
            var request = await cmp.GetRequestAsync(provider);
            var gateway = cmp.GetServiceGateway(request);
            //   var session = await request.GetSessionAsync(true);
            
           
            await HostContext.AppHost.ApplyPreAuthenticateFiltersAsync(request, request.Response);
            await HostContext.ApplyRequestFiltersAsync(request, request.Response, requestDto);
            var response = await gateway.SendAsync(requestDto);
            await HostContext.ApplyResponseFiltersAsync(request, request.Response, response);
            return response;
        }


        public static async Task SendAsync(this IComponent cmp, AuthenticationStateProvider provider, IReturnVoid requestDto, CancellationToken token = default)
        {
            var request = await cmp.GetRequestAsync(provider);
            var gateway = cmp.GetServiceGateway(request);
            //   var session = await request.GetSessionAsync(true);


            await HostContext.AppHost.ApplyPreAuthenticateFiltersAsync(request, request.Response);
            await HostContext.ApplyRequestFiltersAsync(request, request.Response, requestDto);
            await gateway.SendAsync<string>(requestDto); 
        }
    }
}