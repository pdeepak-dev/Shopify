using Npgsql;
using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDb<T>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using var scope = host.Services.CreateScope();

            var serviceProvider = scope.ServiceProvider;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var logger = serviceProvider.GetRequiredService<ILogger<T>>();

            try
            {
                logger.LogInformation("Migrating postresql database.");

                using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                connection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };

                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();

                logger.LogInformation("Migrated postresql database.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the postresql database");

                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    Thread.Sleep(2000);
                    MigrateDb<T>(host, retryForAvailability);
                }
            }

            return host;
        }
    }
}