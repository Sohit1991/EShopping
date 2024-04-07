using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class DbExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder,
                                                      int? retry = 0)
            where TContext : DbContext
        {
            int retryForAVilablity = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    logger.LogInformation($" Started Db Migration: {typeof(TContext).Name}");
                    CallSeeder(seeder, context, services);
                    logger.LogInformation($"Migration Completed : {typeof(TContext).Name}");
                }
                catch (SqlException e)
                {
                    logger.LogError($"An error occurred while migrating db {e} : {typeof(TContext).Name}");
                    if (retryForAVilablity < 4)
                    {
                        retryForAVilablity++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, seeder, retryForAVilablity);
                    }
                }

            }
            return host;
        }

        private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
