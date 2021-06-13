using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Web;
using System.Threading.Tasks;

namespace MyApp
{
    public static class ComponentExtensions
    {
        public static  IServiceStackClient GetServiceStackClient(this ComponentBase _, AuthenticationState state)
        {
            return new ServiceStackClient(state);
        }

        public static async Task<IHttpFile> ToHttpFileAsync(this IBrowserFile browserFile)
        {
            var strm = await browserFile.OpenReadStream(long.MaxValue).CopyToNewMemoryStreamAsync();
            return new HttpFile()
            {
                FileName = browserFile.Name,
                ContentLength = strm.Length,
                ContentType = browserFile.ContentType,
                InputStream = strm
            };
        }
    }
}
