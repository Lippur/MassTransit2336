using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit2336
{
    public class Startup
    {
        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services)
        {
            services.AddMassTransitHostedService();
        }

        public void ConfigureContainer (ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(PongFilter<>))
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterType<PingConsumer>()
                .As<IConsumer<Ping>>()
                .InstancePerLifetimeScope();
            
            builder.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<PingConsumer>();
                cfg.UsingInMemory((context, config) =>
                {
                    AutofacFilterExtensions.UseSendFilter(config, typeof(PongFilter<>), context);
                    config.ConfigureEndpoints(context);
                });
            });
            
            builder.AddMediator(cfg =>
            {
                cfg.AddConsumer<PingConsumer>();
                cfg.ConfigureMediator((context, config) =>
                {
                    AutofacFilterExtensions.UseSendFilter(config, typeof(PongFilter<>), context);
                    config.ConfigureConsumer(context, typeof(PingConsumer));
                });
            });
        }
        
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {}
    }
}