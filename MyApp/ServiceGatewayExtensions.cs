using ServiceStack;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp
{
    public static class ServiceGatewayExtensions
    {
        public static Task<TResponse> SendAsync<TResponse>(this IServiceGatewayAsync gw, IReturn<TResponse> requestDto, CancellationToken token = default)
        {
            return gw.SendAsync<TResponse>((object)requestDto, token);
        }
    }
}