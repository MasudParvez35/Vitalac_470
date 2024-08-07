using System.Reflection;
using FluentMigrator.Infrastructure;
using Nop.Core.Infrastructure;
using Nop.Data.Migrations;

namespace Nop.Web.Framework.Vitalac.Migrations;

public class MigrationManagerDecorator : IMigrationManager
{
    private static bool _initialized;
    private readonly IMigrationManager _decorated;

    public MigrationManagerDecorator(IMigrationManager decorated)
    {
        _decorated = decorated;
    }

    private static async Task InitDatabaseAsync()
    {
        if (_initialized)
            return;

        try
        {
            var baseMigrations = EngineContext.Current.ResolveAll<IBaseMigration>();

            foreach (var baseMigration in baseMigrations)
                await baseMigration.InitAsync();
        }
        finally
        {
            _initialized = true;
        }
    }

    public void ApplyDownMigration(IMigrationInfo migration)
    {
        _decorated.ApplyDownMigration(migration);
    }

    public void ApplyDownMigrations(Assembly assembly)
    {
        _decorated.ApplyDownMigrations(assembly);
    }

    public void ApplyUpMigration(IMigrationInfo migration, bool commitVersionOnly = false)
    {
        _decorated.ApplyUpMigration(migration, commitVersionOnly);
    }

    public void ApplyUpMigrations(Assembly assembly, MigrationProcessType migrationProcessType = MigrationProcessType.Installation, bool commitVersionOnly = false)
    {
        _decorated.ApplyUpMigrations(assembly, migrationProcessType, commitVersionOnly);

        if (assembly == Assembly.GetAssembly(typeof(IMigrationManager)))
            InitDatabaseAsync().Wait();
    }

    public void ApplyUpSchemaMigrations(Assembly assembly)
    {
        _decorated.ApplyUpSchemaMigrations(assembly);
    }
}
