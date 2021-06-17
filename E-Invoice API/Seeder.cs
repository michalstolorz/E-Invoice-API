using E_Invoice_API.Data;
using E_Invoice_API.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Invoice_API
{
    public static class Seeder
    {
        public static void SeedDb(this IApplicationBuilder app, bool development = false)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
            {
                context.Seed();
            }
        }

        private static void Seed(this ApplicationDbContext context)
        {
            var result = context.User.Where(x => (x.FirstName + " " + x.LastName).Equals("Janusz Tracz"));
            if (!result.Any())
            { 
                SeedConfiguration.FirstSeed(context);
            }
        }
    }
}
