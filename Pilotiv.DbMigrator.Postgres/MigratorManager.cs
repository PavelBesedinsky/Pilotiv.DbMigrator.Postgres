using System;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Pilotiv.DbMigrator.Postgres.Options;

namespace Pilotiv.DbMigrator.Postgres;

public static class MigratorManager
{
    public static Task<RunStatus> RunMigratorAsync(Assembly assemblyWithMigrations, params string[] commandLine)
    {
        return Parser.Default.ParseArguments<CheckOptions, UpOptions, DownOptions>(commandLine)
            .MapResult<CheckOptions, UpOptions, DownOptions, Task<RunStatus>>(
                HandleCheckOptionsAsync,
                HandleUpOptionsAsync,
                HandleDownOptionsAsync,
                _ => throw new ArgumentOutOfRangeException());

        Task<RunStatus> HandleCheckOptionsAsync(CheckOptions options)
        {
            return Task.FromResult(GetMigrationRunner(options, assemblyWithMigrations).CheckDatabase(options));
        }

        Task<RunStatus> HandleUpOptionsAsync(UpOptions options)
        {
            return GetMigrationRunner(options, assemblyWithMigrations).UpDatabaseAsync(options);
        }

        Task<RunStatus> HandleDownOptionsAsync(DownOptions options)
        {
            return Task.FromResult(GetMigrationRunner(options, assemblyWithMigrations).DownDatabase(options));
        }
    }

    private static MigrationRunner GetMigrationRunner(DbSettingsOptions dbSettingsOptions,
        Assembly assemblyWithMigrations)
    {
        var serviceProvider = new ServiceCollection()
            .AddMigration(dbSettingsOptions.ConnectionString, assemblyWithMigrations)
            .BuildServiceProvider(false);

        return serviceProvider.GetService<MigrationRunner>() ?? throw new NullReferenceException();
    }
}