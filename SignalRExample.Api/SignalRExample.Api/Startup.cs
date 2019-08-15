using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SignalRExample.Api.Middlewares;
using SignalRExample.Api.SignalR;

namespace SignalRExample.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR(options => { options.EnableDetailedErrors = true; });

            var serviceProvider = ConfigureAutofac(services);
            return serviceProvider;
        }

        private IServiceProvider ConfigureAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMvc();

            app.UseSignalR(builder =>
            {
                builder.MapHub<ChatHub>("/hubs/notifications",
                    options => options.Transports = HttpTransportType.WebSockets);
            });
        }
    }
}