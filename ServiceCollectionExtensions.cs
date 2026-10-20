using HH.YiDASDK;
using HH.YiDASDK.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>注册 SDK；配置文件及其优先级由宿主管理。</summary>
    public static IServiceCollection AddYiDASdk(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        AddCore(services).Bind(configuration);
        return services;
    }

    /// <summary>通过委托配置 SDK。</summary>
    public static IServiceCollection AddYiDASdk(this IServiceCollection services, Action<YiDAConfig> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        AddCore(services).Configure(configure);
        return services;
    }

    /// <summary>兼容旧注册名称；宿主需自行加载 appsettings.YiDA.json。</summary>
    [Obsolete("Load configuration in the host, then call AddYiDASdk(configuration.GetSection(YiDAConfig.SectionName)).")]
    public static IServiceCollection AddYiDASDKSetup(this IServiceCollection services, IConfiguration configuration)
        => services.AddYiDASdk(configuration.GetSection(YiDAConfig.SectionName));

    private static OptionsBuilder<YiDAConfig> AddCore(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IValidateOptions<YiDAConfig>, YiDAConfigValidator>());
        services.TryAddSingleton(TimeProvider.System);
        // 独立的命名客户端，避免附件请求携带网关签名或记录临时附件地址。
        services.AddHttpClient(YiDAHttpClientNames.Api).RemoveAllLoggers();
        services.AddHttpClient(YiDAHttpClientNames.Download).RemoveAllLoggers();
        services.TryAddKeyedSingleton<RateLimiter>(YiDAHttpClientNames.Api, (_, _) =>
            new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromSeconds(1),
                SegmentsPerWindow = 10,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = int.MaxValue,
                AutoReplenishment = true
            }));
        services.TryAddScoped<IYiDAClient, DefaultYiDAClient>();
        // 保留直接注入 YiDAConfig 的用法；每个作用域取得一致的配置快照。
        services.TryAddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<YiDAConfig>>().Value);
        return services.AddOptions<YiDAConfig>().ValidateOnStart();
    }
}
