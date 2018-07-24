using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using Zametek.Utility.Logging.AspNetCore;

namespace Company.Api.Rest.Impl
{
    public class Startup
    {
        public const string RemoteIpAddressName = nameof(ConnectionInfo.RemoteIpAddress);
        public const string TraceIdentifierName = nameof(HttpContext.TraceIdentifier);
        public const string UserIdName = @"UserId";
        public const string ConnectionIdName = @"ConnectionId";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddMvc();
            //services.AddRouting(options => // This doesn't work yet for some reason.
            //{
            //    options.LowercaseUrls = true;
            //});
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.DocumentFilter<LowercaseDocumentFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseTrackingMiddleware(
                (context) => new Dictionary<string, string>()
                {
                    { RemoteIpAddressName, context.Connection?.RemoteIpAddress?.ToString() },
                    { TraceIdentifierName, context.TraceIdentifier },
                    { UserIdName, context.User?.Identity?.Name },
                    { ConnectionIdName, context.Connection.Id },
                    { "Jurisdiction", "UK" },
                    { "New call-specific random string", System.Guid.NewGuid().ToString() }
                });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company - API");
            });
        }
    }
}
