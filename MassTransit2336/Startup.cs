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

        public void ConfigureServices (IServiceCollection services)
        {
            services.AddScoped(typeof(PongFilter<>));
            services.AddScoped<PingConsumer>();
            
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<PingConsumer>();
                cfg.UsingInMemory((context, config) =>
                {
                    config.UseSendFilter(typeof(PongFilter<>), context);
                    config.ConfigureEndpoints(context);
                });
            });
            
            services.AddMediator(cfg =>
            {
                cfg.AddConsumer<PingConsumer>();
                cfg.ConfigureMediator((context, config) =>
                {
                    config.UseSendFilter(typeof(PongFilter<>), context);
                });
            });
            
            services.AddMassTransitHostedService();
        }
        
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env)
        {}
    }
}