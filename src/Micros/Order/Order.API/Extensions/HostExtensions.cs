using System;
using System.Threading;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Order.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext,
            IServiceProvider> seeder, int? retry = 10)
            where TContext : DbContext
        {
            int retryForAvailability = retry.Value;

            using var scope = host.Services.CreateScope();
            var provider = scope.ServiceProvider;
            var logger = provider.GetRequiredService<ILogger<TContext>>();
            var ctx = provider.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                InvokeSeeder(seeder, ctx, provider);

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (SqlException sqlEx)
            {
                logger.LogError(sqlEx, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDatabase(host, seeder, retryForAvailability);
                }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext ctx, IServiceProvider provider) where TContext : DbContext
        {
            ctx.Database.Migrate();
            seeder(ctx, provider);
        }
    }
}
