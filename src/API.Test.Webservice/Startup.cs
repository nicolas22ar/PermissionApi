using API.Test.Infrastructure.Concrete.DbContexts;
using API.Test.Infrastructure.Concrete.Mappers;
using API.Test.Infrastructure.Concrete.Repositories;
using API.Test.Infrastructure.Concrete.Services;
using API.Test.Infrastructure.Repositories;
using API.Test.Infrastructure.Services;
using API.Test.WebService.Middlewares;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace API.Test.WebService
{
    /// <summary>
    /// Runtime startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Runtime startup
        /// </summary>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Application configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TestDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("TestDb")));

            services
                .AddControllers()
                .AddNewtonsoftJson();

            // swagger
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("TestAPI",
                    new OpenApiInfo()
                    {
                        Title = "Test API",
                        Version = "1"
                    });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionService, PermissionService>();

            var autoMapconfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            services.AddSingleton(s => autoMapconfig.CreateMapper());
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        public static void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseErrorHandling(Log.Logger);

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/TestAPI/swagger.json", "Test API");
                setupAction.RoutePrefix = "";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}