namespace Nop.Web.Framework.Vitalac.Migrations;

public interface IBaseMigration
{
    Task InitAsync();

    int Order { get; }
}
