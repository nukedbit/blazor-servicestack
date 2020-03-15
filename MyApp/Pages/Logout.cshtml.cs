using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceStack;
using ServiceStack.Host.NetCore;

namespace MyApp.Pages
{
    public class Logout : PageModel
    {

        public Logout(IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await Gateway.SendAsync(new Authenticate()
            {
                provider = "logout"
            });
            return LocalRedirect("/");
        }

      
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        private IServiceStackProvider _serviceStackProvider;
        protected virtual IServiceStackProvider Provider
        {
            get
            {
                if (_serviceStackProvider != null)
                {
                    return _serviceStackProvider;
                }

                var netCoreRequest = new NetCoreRequest(this.HttpContextAccessor.HttpContext, GetType().Name);
                netCoreRequest.SetInProcessRequest();
                _serviceStackProvider =
                    new ServiceStackProvider(netCoreRequest);
                return _serviceStackProvider;
            }
        }


        public IServiceGateway Gateway => Provider.Gateway;
    }
}