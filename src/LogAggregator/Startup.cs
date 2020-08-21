using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LogAggregator.Configuration;
using LogAggregator.Domain.Clients;
using LogAggregator.Domain.Handlers;
using LogAggregator.Domain.Manager;
using LogAggregator.GrpcServices;
using LogAggregator.Persistence;
using Swisschain.Sdk.Server.Common;

namespace LogAggregator
{
    public sealed class Startup : SwisschainStartup<AppConfig>
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override void ConfigureServicesExt(IServiceCollection services)
        {
            base.ConfigureServicesExt(services);

            //services.AddPersistence(Config.Db.ConnectionString);

            //services.AddHostedService<IHostedService>()

        }

        protected override void ConfigureContainerExt(ContainerBuilder builder)
        {
            builder.RegisterType<LogManager>()
                .As<ILogManager>()
                .SingleInstance();

            builder.RegisterType<SimpleHandler>()
                .As<ILogHandler>();

            builder.RegisterType<ElasticSearchHandler>()
                .As<ILogHandler>();

            builder.RegisterInstance(new ElkClient(Config.ElasticSearchSettings))
                .AsSelf()
                .SingleInstance();

        }

        protected override void RegisterEndpoints(IEndpointRouteBuilder endpoints)
        {
            base.RegisterEndpoints(endpoints);

            endpoints.MapGrpcService<MonitoringService>();
        }
    }
}
