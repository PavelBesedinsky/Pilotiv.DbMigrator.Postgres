using CommandLine;

namespace Pilotiv.DbMigrator.Postgres.Options;

public class DbSettingsOptions
{
    [Option("host", Required = true, HelpText = "Specifying database host.")]
    public string? Host { get; set; }

    [Option("port", Required = true, HelpText = "Specifying database port.")]
    public string? Port { get; set; }

    [Option("db", Required = true, HelpText = "Specifying database name.")]
    public string? Database { get; set; }

    [Option("user", Required = true, HelpText = "Specifying database user id.")]
    public string? UserId { get; set; }

    [Option("password", Required = true, HelpText = "Specifying database user password.")]
    public string? Password { get; set; }

    public string ConnectionString =>
        $"Host={Host};Port={Port};Database={Database}; Username={UserId}; Password={Password};";
}