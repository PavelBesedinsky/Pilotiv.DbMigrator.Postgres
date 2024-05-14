using System;
using FluentMigrator.Runner;
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

    public RunStatus UpDatabase(UpOptions options)
    {
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
        try
        {
            action();
            return RunStatus.Ok;
        }
        catch (Exception)
        {
            return RunStatus.Error;
        }
    }
}