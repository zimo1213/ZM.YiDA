using HH.YiDASDK.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace HH.YiDASDK;

/// <summary>宜搭客户端。每次调用固定配置和参数，重试时重新签名。</summary>
public class DefaultYiDAClient(
    IHttpClientFactory httpClientFactory,
    IOptionsSnapshot<YiDAConfig> options,
    ILogger<DefaultYiDAClient> logger,
    TimeProvider timeProvider,
    [FromKeyedServices(YiDAHttpClientNames.Api)] RateLimiter rateLimiter) : IYiDAClient
{
    public async Task<T> ExecuteAsync<T>(IYiDARequest<T> request, CancellationToken cancellationToken = default)
        where T : YiDAResponse
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();
        request.Validate();
        var config = options.Value;
        var pairs = request.ToDictionaryOfStringValue();
        var serverUrl = request.GetUrl();
        using var client = httpClientFactory.CreateClient(YiDAHttpClientNames.Api);

        for (var attempt = 1; ; attempt++)
        {
            using var lease = await rateLimiter.AcquireAsync(1, cancellationToken).ConfigureAwait(false);
            if (!lease.IsAcquired)
                throw new YiDAException("YiDA request could not acquire a rate-limit permit.");
            cancellationToken.ThrowIfCancellationRequested();
            var (response, statusCode) = await InvokeAsync<T>(client, pairs, serverUrl, config, cancellationToken)
                .ConfigureAwait(false);
            if (response.success)
            {
                if (response is GetOpenUrlResponse download)
                    download.AttachDownloadClient(httpClientFactory);
                return response;
            }

            bool retry = response.errorCode == "TIANSHU_000061"
                || (response.errorCode == "100"
                    && response.Message?.Contains("timeout", StringComparison.OrdinalIgnoreCase) == true);
            if (!retry || attempt >= config.ClientConfig.RetryLimit)
                throw new YiDAException($"YiDA request failed ({response.errorCode}): {response.ErrorInfo}")
                {
                    Response = response,
                    StatusCode = statusCode
                };

            logger.LogWarning("YiDA request will retry after business error {ErrorCode}; attempt {Attempt}/{AttemptLimit}",
                response.errorCode, attempt, config.ClientConfig.RetryLimit);
            await Task.Delay(TimeSpan.FromMilliseconds(config.ClientConfig.RetryDelayMilliseconds),
                timeProvider, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<(T Response, HttpStatusCode StatusCode)> InvokeAsync<T>(
        HttpClient client, IDictionary<string, string> pairs, string serverUrl, YiDAConfig config,
        CancellationToken cancellationToken) where T : YiDAResponse
    {
        var uri = new Uri($"{config.BaseUrl.TrimEnd('/')}/{serverUrl.TrimStart('/')}");
        using var message = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = new FormUrlEncodedContent(pairs)
        };
        var timestamp = timeProvider.GetUtcNow().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
        message.Headers.Add("X-Hmac-Auth-Timestamp", timestamp);
        message.Headers.Add("X-Hmac-Auth-Version", config.Version);
        message.Headers.Add("X-Hmac-Auth-Nonce", config.Nonce);
        message.Headers.Add("X-Hmac-Auth-Signature",
            YiDASignatureUtil.Signature(pairs, timestamp, config.Nonce, serverUrl, config.Secret));
        message.Headers.Add("apiKey", config.Key);

        // 默认读取完整的小型 JSON 响应，使 HttpClient.Timeout 覆盖响应正文读取。
        using var result = await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
        var body = await result.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        T response;
        try
        {
            response = body.Deserialize<T>();
        }
        catch (JsonException exception)
        {
            if (!result.IsSuccessStatusCode)
                result.EnsureSuccessStatusCode();
            throw new YiDAException("YiDA returned an invalid JSON response.", exception)
            {
                StatusCode = result.StatusCode
            };
        }
        response.body = body;
        // 非 2xx 仍保留合法的宜搭错误响应，供业务重试和异常诊断使用。
        if (!result.IsSuccessStatusCode && response.success)
            result.EnsureSuccessStatusCode();
        return (response, result.StatusCode);
    }
}
