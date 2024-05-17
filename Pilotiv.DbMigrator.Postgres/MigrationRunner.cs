using System;
using System.Threading.Tasks;
using Dapper;
using FluentMigrator.Runner;
using Npgsql;
using Pilotiv.DbMigrator.Postgres.Options;

namespace Pilotiv.DbMigrator.Postgres;

public class MigrationRunner
{
    private readonly IMigrationRunner _migrationRunner;

    public MigrationRunner(IMigrationRunner migrationRunner)
    {
        _migrationRunner = migrationRunner;
    }

    public RunStatus CheckDatabase(CheckOptions options)
    {
        return TryRun(_migrationRunner.ValidateVersionOrder);
    }

    public async Task<RunStatus> UpDatabaseAsync(UpOptions options)
    {
        await CreateDbAsync(options);

        if (options.UpTo.HasValue)
        {
            return TryRun(() => _migrationRunner.MigrateUp(options.UpTo.Value));
        }

        return TryRun(_migrationRunner.MigrateUp);
    }

    public RunStatus DownDatabase(DownOptions options)
    {
        return TryRun(() => _migrationRunner.MigrateDown(options.DownTo));
    }

    private static RunStatus TryRun(Action action)
    {
        action();
        return RunStatus.Ok;
    }

    private static async Task CreateDbAsync(DbSettingsOptions options)
    {
        await using var connection = new NpgsqlConnection(options.MasterConnectionString);

        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{options.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount is 0)
        {
            var sql = $"CREATE DATABASE \"{options.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }
}