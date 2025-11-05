namespace HH.YiDASDK.Core;

/// <summary>
/// 宜搭配置
/// </summary>
public class YiDAConfig
{
    /// <summary>
    /// 开发账户的 key 
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// 开发账户的 secret 
    /// </summary>
    public string Secret { get; set; }
    public string BaseUrl { get; set; }
    public string Version { get; set; }
    public string Nonce { get; set; }
    public YiDAClientConfig ClientConfig { get; set; }
    public Dictionary<string, YiDARequest> RequestMap { get; set; }
    public class YiDAClientConfig
    {
        public int RetryLimit { get; set; }
        public int RetryDelayMilliseconds { get; set; }
    }

    public class YiDARequest
    {
        public DefaultYiDARequest Request { get; set; }
        public Dictionary<string, string> FormUuidMap { get; set; }

        public DefaultYiDARequest GetByFormUuid(string formUuid)
        {
            return Request.SetFormUuid(FormUuidMap[formUuid]);
        }
    }

    /// <summary>
    /// 如果只有一个配置,则默认第一个,传入GetByFormUuid("order)
    /// 如果多个,传入GetByFormUuid("yida:order")
    /// </summary>
    /// <param name="formUuid"></param>
    /// <returns></returns>
    public DefaultYiDARequest GetByFormUuid(string key)
    {
        string[] split = [.. key.Split(":")];
        split.Reverse();
        YiDARequest request = RequestMap.Values.First();
        if (split.Length > 1)
        {
            request = RequestMap[split[1]];
        }
        return request.GetByFormUuid(split[0]);
    }
}