using System.Xml;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Services.Localization;

namespace Nop.Web.Framework.Vitalac.Migrations;

public partial class StringResourceMigration : IBaseMigration
{
    private readonly ILocalizationService _localizationService;
    private readonly ILanguageService _languageService;
    private readonly INopFileProvider _fileProvider;

    public StringResourceMigration(ILocalizationService localizationService,
        ILanguageService languageService,
        INopFileProvider nopFileProvider)
    {
        _localizationService = localizationService;
        _languageService = languageService;
        _fileProvider = nopFileProvider;
    }

    public async Task InitAsync()
    {
        foreach (var language in await _languageService.GetAllLanguagesAsync(true))
            foreach (var resource in GetStringResources(language.LanguageCulture))
                if (await _localizationService.GetLocaleStringResourceByNameAsync(resource.Key, language.Id, false) is null)
                {
                    var lsr = new LocaleStringResource
                    {
                        LanguageId = language.Id,
                        ResourceName = resource.Key,
                        ResourceValue = resource.Value
                    };
                    await _localizationService.InsertLocaleStringResourceAsync(lsr);
                }
    }

    public int Order => 1;

    protected IDictionary<string, string> GetStringResources(string languageCulture)
    {
        var resourceStrings = new Dictionary<string, string>();

        var filePath = _fileProvider.MapPath($"~/App_Data/Localization/Migration/migration.{languageCulture}.xml");
        if (!_fileProvider.FileExists(filePath))
            return resourceStrings;

        var xmlDocument = new XmlDocument();
        xmlDocument.Load(filePath);

        var languageNode = xmlDocument.SelectSingleNode(@"//Language");

        if (languageNode == null || languageNode.Attributes == null)
            return resourceStrings;

        //load resources
        var resources = xmlDocument.SelectNodes(@"//Language/LocaleResource");
        if (resources == null)
            return resourceStrings;

        foreach (XmlNode resNode in resources)
        {
            if (resNode.Attributes == null)
                continue;

            var resNameAttribute = resNode.Attributes["Name"];
            var resValueNode = resNode.SelectSingleNode("Value");

            if (resNameAttribute == null)
                throw new NopException("All migration resources must have an attribute Name=\"Value\".");
            var resourceName = resNameAttribute.Value.Trim();
            if (string.IsNullOrEmpty(resourceName))
                throw new NopException("All migration resource attributes 'Name' must have a value.'");

            if (resValueNode == null)
                throw new NopException("All migration resources must have an element \"Value\".");
            var resourceValue = resValueNode.InnerText.Trim();

            resourceStrings[resourceName] = resourceValue;
        }

        return resourceStrings;
    }
}
