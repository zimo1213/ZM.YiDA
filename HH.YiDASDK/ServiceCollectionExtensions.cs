using Flurl.Http.Configuration;
using HH.YiDASDK;
using HH.YiDASDK.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYiDASDKSetup(this IServiceCollection service, ConfigurationManager configuration)
    {
        configuration.AddJsonFile("appsettings.YiDA.json", optional: true, reloadOnChange: true);
        service.Configure<YiDAConfig>(configuration.GetSection(nameof(YiDAConfig)));
        service.AddSingleton(opt => opt.GetRequiredService<IOptions<YiDAConfig>>().Value);
        service.TryAddScoped<IYiDAClient, DefaultYiDAClient>();
        service.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();

        return service;
    }
}
