using CommandLine;

namespace Pilotiv.DbMigrator.Postgres.Options;

[Verb("down", HelpText = "Downgrade database.")]
public class DownOptions : DbSettingsOptions
{
    [Option(Required = true, HelpText = "Downgrade to specified migration number.")]
    public int DownTo { get; set; }
}