using NovibetIPStackAPI.IPStackWrapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using NovibetIPStackAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NovibetIPStackAPI.WebApi.Services;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces;
using NovibetIPStackAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Design;
using NovibetIPStackAPI.Infrastructure.Persistence;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.IPRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.IPRelated;
using System.Runtime.CompilerServices;
using NovibetIPStackAPI.WebApi.Services.Interfaces;
using NovibetIPStackAPI.Infrastructure.Repositories.Interfaces.BatchRelated;
using NovibetIPStackAPI.Infrastructure.Repositories.BatchRelated;
using NovibetIPStackAPI.WebApi;

namespace Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddIPInfoProvider();
            services.AddInfrastructure(Configuration);
            services.InjectRepositories();
            services.InjectWebApiServices();
            services.AddTransient<IIPDetailsRepository, IPDetailsRepository>();


            services.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
                cfg.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "Tichu Sensei API",
                        Version = "v1.0",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "ch.asimakopoulos@gmail.com",
                            Name = "Charalampos Asimakopoulos",
                            Url = new System.Uri("https://github.com/ch-asimakopoulos/NovibetIPStackAPI")
                        }
                    });

                c.CustomSchemaIds(type => type.Name.EndsWith("DTO") ? type.Name.Replace("DTO", string.Empty) : type.Name);

            });

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Novibet IP Geolocation API v1.0");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
