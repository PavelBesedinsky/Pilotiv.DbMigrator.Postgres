using System;
using System.Reflection;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Pilotiv.DbMigrator.Postgres.Options;

namespace Pilotiv.DbMigrator.Postgres;

public static class MigratorManager
{
    public static int RunMigrator(Assembly assemblyWithMigrations, params string[] commandLine)
    {
        return Parser.Default.ParseArguments<CheckOptions, UpOptions, DownOptions>(commandLine)
            .MapResult(
                (CheckOptions options) => (int) GetMigrationRunner(options, assemblyWithMigrations).CheckDatabase(options),
                (UpOptions options) => (int) GetMigrationRunner(options, assemblyWithMigrations).UpDatabase(options),
                (DownOptions options) => (int) GetMigrationRunner(options, assemblyWithMigrations).DownDatabase(options),
                _ => 1);
    }

    private static MigrationRunner GetMigrationRunner(DbSettingsOptions dbSettingsOptions, Assembly assemblyWithMigrations)
    {
        var serviceProvider = new ServiceCollection()
            .AddMigration(dbSettingsOptions.ConnectionString, assemblyWithMigrations)
            .BuildServiceProvider(false);

        return serviceProvider.GetService<MigrationRunner>() ?? throw new NullReferenceException();
    }
}