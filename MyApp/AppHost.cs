using Funq;
using Microsoft.AspNetCore.Hosting;
using MyApp.ServiceInterface;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Validation;

namespace MyApp
{
    public class AppHost : AppHostBase
    {
        public AppHost()
            : base("MyApp", typeof(ForecastService).Assembly) { }

        public override void Configure(Container container)
        {
            container.RegisterAs<OrmLiteCacheClient, ICacheClient>();
            container.Resolve<ICacheClient>().InitSchema();
            Plugins.Add(new ValidationFeature());
            Plugins.Add(new AutoQueryFeature { MaxLimit = 100 });
            Plugins.Add(new PostmanFeature());

            SetConfig(new HostConfig
            {
                UseSameSiteCookies = true,
                AddRedirectParamsToQueryString = true,
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), HostingEnvironment.IsDevelopment()),
            });

            Plugins.Add(new CorsFeature());
           
        }
    }
}