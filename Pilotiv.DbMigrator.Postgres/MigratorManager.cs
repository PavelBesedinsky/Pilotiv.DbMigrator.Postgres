using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Pilotiv.DbMigrator.Postgres.Options;

namespace Pilotiv.DbMigrator.Postgres;

public static class MigratorManager
{
    public static int RunMigrator(params string[] commandLine)
    {
        return Parser.Default.ParseArguments<CheckOptions, UpOptions, DownOptions>(commandLine)
            .MapResult(
                (CheckOptions options) => (int) GetMigrationRunner(options).CheckDatabase(options),
                (UpOptions options) => (int) GetMigrationRunner(options).UpDatabase(options),
                (DownOptions options) => (int) GetMigrationRunner(options).DownDatabase(options),
                _ => 1);
    }

    private static MigrationRunner GetMigrationRunner(DbSettingsOptions dbSettingsOptions)
    {
        var serviceProvider = new ServiceCollection()
            .AddMigration(dbSettingsOptions.ConnectionString)
            .BuildServiceProvider(false);

        return new MigrationRunner(serviceProvider);
    }
}