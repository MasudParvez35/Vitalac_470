using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Data.Migrations;
using Nop.Web.Framework.Vitalac.Migrations;

namespace Nop.Web.Framework.Vitalac.Infrastructure;

public partial class NopStartup : INopStartup
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Decorate<IMigrationManager, MigrationManagerDecorator>();

        //find startup tasks provided by other assemblies
        var typeFinder = Singleton<ITypeFinder>.Instance;

        foreach (var type in typeFinder.FindClassesOfType<IBaseMigration>())
            services.AddTransient(typeof(IBaseMigration), type);
    }

    public void Configure(IApplicationBuilder application)
    {
    }

    public int Order => 100;
}