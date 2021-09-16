using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                #if DEBUG
                Console.WriteLine("Seeding Data...");
                #endif
                context.Platforms.AddRange(
                    new Platform {Name = "DotNet", Publisher = "Microsoft", Cost = "Free"}, 
                    new Platform{Name = "Docker", Publisher = "Docker", Cost = "Free"},
                    new Platform{Name = "Coca Cola", Publisher = "Coca Cola Company", Cost = "Free"}
                    );      
                context.SaveChanges();
            }
            else
            {   
                #if DEBUG
                Console.WriteLine("contains already data");
                #endif
            }
        } 
    }
}