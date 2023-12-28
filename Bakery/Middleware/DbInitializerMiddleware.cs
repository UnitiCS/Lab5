using Bakery.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next) => _next = next;

        // В DbInitializerMiddleware.cs
        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider, BakeryDBContext dbContext)
        {
            if (context.Session != null && !(context.Session.Keys.Contains("starting")))
            {
                DbInitializer.Initialize(dbContext);
                context.Session.SetString("starting", "Yes");

                Console.WriteLine("База данных была успешно инициализирована.");
            }
            else
            {
                Console.WriteLine("База данных уже инициализирована. Пропускаем инициализацию.");
            }

            //Console.WriteLine($"Session key 'starting' value: {context.Session.GetString("starting")}");

            // Call the next delegate/middleware in the pipeline
            await _next.Invoke(context);
        }
    }

        public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbInitializerMiddleware>();
        }
    }
}
