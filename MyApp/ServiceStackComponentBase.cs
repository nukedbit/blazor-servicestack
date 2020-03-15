using System;
using System.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using ServiceStack;
using ServiceStack.Host.NetCore;
using ServiceStack.Web;

namespace MyApp
{
    public partial class ServiceStackComponentBase : ComponentBase, IDisposable
    {
        [Inject]
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

        public virtual IHttpRequest Request => Provider.Request;

        public IDbConnection Db => Provider.Db;

        public virtual void Dispose()
        {
            if (_serviceStackProvider != null)
            {
                EndServiceStackRequest();
                _serviceStackProvider.Dispose();
                _serviceStackProvider = null;
            }
        }

        public virtual void EndServiceStackRequest() =>
            HostContext.AppHost.OnEndRequest(Request);
    }
}