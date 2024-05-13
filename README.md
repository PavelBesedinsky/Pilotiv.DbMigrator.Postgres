### Description

Library based on Fluent Migrator to run migration to PostgreSQL database.

#### Usage

Pilotiv.DbMigrator.Postgres provides main commands:

`check` - to validate that there were no missing migration versions
<details><summary>check command arguments</summary>

```
  --host        Required. Specifying database host.

  --port        Required. Specifying database port.

  --db          Required. Specifying database name.

  --user        Required. Specifying database user id.

  --password    Required. Specifying database user password.
```
</details>

`up` - to update database

<details><summary>up command arguments</summary>

```
  --upto        Update up to specified migration number(included).

  --host        Required. Specifying database host.

  --port        Required. Specifying database port.

  --db          Required. Specifying database name.

  --user        Required. Specifying database user id.

  --password    Required. Specifying database user password.

  --help        Display this help screen.

  --version     Display version information.
```
</details>

`down` - to downgrade database

<details><summary>down command arguments</summary>

```
  --downto      Required. Downgrade to specified migration number.

  --host        Required. Specifying database host.

  --port        Required. Specifying database port.

  --db          Required. Specifying database name.

  --user        Required. Specifying database user id.

  --password    Required. Specifying database user password.

  --help        Display this help screen.

  --version     Display version information.
```
</details>

It can be run through the `MigratorManager.RunMigrator(commandLine)` method, where `commandLine` is one of the commands 
given above.

Or you can call `DependencyInjection.AddMigration` extension on service collection to add it to your DI. Additionally,
you have to add call handlers of `MigrationRunner` (like `CheckDatabase`, `UpDatabase`, `DownDatabase`) by yourself.

The lib will look for migration classes in executing assembly directories.

P.S. Example of migration classes:
```csharp
[Migration(0, "Initial migration.")]
public class M000000_InitialMigration : AutoReversingMigration
{
    public override void Up()
    {
    }
}

[Migration(1, "Create First Table.")]
public class M000001_CreateFirstTable : AutoReversingMigration
{
    public override void Up()
    {
        Create
            .Table("users")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("login").AsString(255).Nullable()
            .WithColumn("password_hash").AsString(255).Nullable()
            .WithColumn("email").AsString(255).Nullable()
            .WithColumn("registration_date").AsDateTime().Nullable()
            .WithColumn("authorization_date").AsDateTime().Nullable()
            .WithColumn("vk_user_id").AsGuid().Nullable();
    }
}
```

