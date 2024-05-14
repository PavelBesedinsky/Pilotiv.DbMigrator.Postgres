using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Pilotiv.DbMigrator.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddMigration(this IServiceCollection services, string connectionString, Assembly assemblyWithMigrations)
    {
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(assemblyWithMigrations).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        services.AddScoped<MigrationRunner>();
        
        return services;
    }
}