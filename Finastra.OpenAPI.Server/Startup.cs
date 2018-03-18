using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen; // for ApiDescription.ControllerAttributes

namespace Finastra.OpenAPI.Server
{
    public class Startup
    {
        private readonly List<string> _controllerAssemblies;
        private readonly List<string> _apiVersions;
        private readonly string _apiName;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _apiName = Configuration["Finastra:OpenAPI:Name"];
            _controllerAssemblies = Configuration.GetSection("Finastra:OpenAPI:Assemblies").Get<List<string>>();
            _apiVersions = Configuration.GetSection("Finastra:OpenAPI:Versions").Get<List<string>>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var mvc = services.AddMvc();
            foreach (var controller in _controllerAssemblies)
                mvc.AddApplicationPart(System.Reflection.Assembly.LoadFrom(controller));
            mvc.AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });
                foreach (var version in _apiVersions)
                {
                    c.SwaggerDoc($"v{version}", new Info { Title = _apiName, Version = $"v{version}" });
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app
                .UseSwagger(c =>
                {
                    c.RouteTemplate = "api-docs/swagger-{documentName}.json";
                })
                .UseSwaggerUI(c =>
                    {
                        c.RoutePrefix = "api-docs";
                        foreach (var version in _apiVersions)
                        {
                            c.SwaggerEndpoint($"/api-docs/swagger-v{version}.json", $"{_apiName} v{version}");
                        }
                    })
                .UseMvc();
        }
    }
}
