using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using platformservice.SyncDataServices.Http;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;
using System;

namespace PlatformService
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
             if (Env.IsProduction())
                 services.AddDbContext<AppDbContext>(opt => 
                     opt.UseSqlServer(Configuration.GetConnectionString("PlatfromsConn")));
             else
             {
                 Console.WriteLine("Using In Memory database");
                 services.AddDbContext<AppDbContext>(opt =>
                     opt.UseInMemoryDatabase("InMem")); 
             }
             
             services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>(); 
             services.AddScoped<IPlatformRepo, PlatformRepo>(); 
             services.AddControllers(); 
             services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
             services.AddSwaggerGen(c =>
             { 
                 c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
             });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
