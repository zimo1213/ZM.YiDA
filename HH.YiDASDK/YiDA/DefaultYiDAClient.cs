using Flurl.Http;
using Flurl.Http.Configuration;
using HH.YiDASDK.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HH.YiDASDK;

/// <summary>
/// SDK实现
/// </summary>
public class DefaultYiDAClient(
    IFlurlClientFactory flurlClientFactory,
    IOptions<YiDAConfig> options,
    ILogger<DefaultYiDAClient> logger) : IYiDAClient
{

    public Task<T> ExecuteAsync<T>(IYiDARequest<T> request) where T : YiDAResponse
    {
        return ExecuteAsyncProxy(request);
    }

    private async Task<T> ExecuteAsyncProxy<T>(IYiDARequest<T> request) where T : YiDAResponse
    {
        int retryCount = 0;
        int millisecondsDelay = options.Value.ClientConfig.RetryDelayMilliseconds;
        int retryLimit = options.Value.ClientConfig.RetryLimit;
        while (retryCount < retryLimit)
        {
            retryCount++;
            millisecondsDelay *= retryCount;
            T rsp = await InvokeAsync(request);
            if (rsp.success)
            {
                return rsp;
            }
            if ("100" == rsp.errorCode && rsp.Message is not null)
            {
                if (rsp.Message.Contains("timeout"))
                {
                    logger.LogWarning("[Timeout] result={},retryCount={}", rsp.body, retryCount);
                    goto DELAY;
                }
                if (rsp.Message.Contains("signature"))
                {
                    logger.LogWarning("[SignatureNonceUsed] result={},retryCount={}", rsp.body, retryCount);
                    break;
                }
            }
            if ("TIANSHU_000061" == rsp.errorCode)
            {
                logger.LogInformation("[Qps] result={},retryCount={}", rsp.body, retryCount);
                goto DELAY;
            }
            logger.LogError("[Fail] result={},retryCount={}", rsp.body, retryCount);
        DELAY:
            await Task.Delay(millisecondsDelay);
        }
        YiDAException.Throw("proxy error!");
        return null;
    }

    /// <summary>
    /// 执行请求
    /// </summary>
    /// <typeparam name="T">基础返回抽象对象</typeparam>
    /// <param name="request">请求对象</param>
    /// <param name="token">身份对象</param>
    /// <returns></returns>
    private async Task<T> InvokeAsync<T>(IYiDARequest<T> request) where T : YiDAResponse
    {
        request.Validate();
        LimiterSingleton.Invoke();
        YiDAConfig config = options.Value;
        string serverUrl = request.GetUrl();
        IDictionary<string, string> pairs = request.ToDictionaryOfStringValue();
        IDictionary<string, string> header = GetHeader(pairs, serverUrl, config);
        IFlurlClient client = flurlClientFactory.Get(config.BaseUrl);
        client.WithHeaders(header);
        string result;
        try
        {
            IFlurlResponse res = await client.Request(serverUrl).PostUrlEncodedAsync(pairs);
            result = await res.GetStringAsync();
        }
        catch (FlurlHttpException ex)
        {
            result = await ex.GetResponseStringAsync();
        }
        T rsp = result.Deserialize<T>();
        rsp.body = result;
        return rsp;
    }

    /// <summary>
    /// 获取请求头
    /// </summary>
    /// <param name="pairs">请求参数</param>
    /// <param name="serverUrl">请求网址</param>
    /// <param name="config">配置对象</param>
    /// <returns></returns>
    private static IDictionary<string, string> GetHeader(IDictionary<string, string> pairs, string serverUrl, YiDAConfig config)
    {
        string timestamp = YiDASignatureUtil.Iso8601Date();
        string sign = YiDASignatureUtil.Signature(pairs, timestamp, config.Nonce, serverUrl, config.Secret);
        return new Dictionary<string, string>
        {
             {  "X-Hmac-Auth-Timestamp",timestamp },
             {  "X-Hmac-Auth-Version" ,config.Version },
             {  "X-Hmac-Auth-Nonce" ,config.Nonce },
             {  "X-Hmac-Auth-Signature" ,sign },
             {  "apiKey",config.Key }
        };
    }
}
