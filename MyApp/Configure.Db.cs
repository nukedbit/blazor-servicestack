using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace MyApp
{
    public class ConfigureDb : IConfigureServices, IConfigureAppHost
    {
        IConfiguration Configuration { get; }
        public ConfigureDb(IConfiguration configuration) => Configuration = configuration;

        public void Configure(IServiceCollection services)
        {
            IOrmLiteDialectProvider provider = SqliteDialect.Provider;
            var mapAbsolutePath = $"~/data.sqlite".MapAbsolutePath();
            services.AddSingleton((IDbConnectionFactory)new OrmLiteConnectionFactory(
                mapAbsolutePath,
                provider));
        }

        public void Configure(IAppHost appHost)
        {
            var dbFactory = appHost.GetContainer().Resolve<IDbConnectionFactory>();
            using var db = dbFactory.CreateDbConnection();
            db.Open();
            db.CreateTableIfNotExists<LogClick>();
        }
    }   
}