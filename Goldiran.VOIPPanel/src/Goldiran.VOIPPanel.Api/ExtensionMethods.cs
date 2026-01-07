using Microsoft.Extensions.Options;
using Voip.Framework.Common.AppSettings;

namespace Goldiran.VOIPPanel.Api;
public static class ExtensionMethods
{
    static SiteSettings GetSiteSettings(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var siteSettingsOptions = provider.GetRequiredService<IOptionsSnapshot<SiteSettings>>();
        var siteSettings = siteSettingsOptions.Value;
        if (siteSettings == null) throw new ArgumentNullException(nameof(siteSettings));
        return siteSettings;
    }
}

