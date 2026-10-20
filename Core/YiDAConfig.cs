namespace HH.YiDASDK.Core;

/// <summary>宜搭配置。配置节结构兼容旧版本。</summary>
public class YiDAConfig
{
    public const string SectionName = "YiDAConfig";

    public string Key { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.aliwork.com";
    public string Version { get; set; } = "1";
    public string Nonce { get; set; } = string.Empty;
    public YiDAClientConfig ClientConfig { get; set; } = new();
    public Dictionary<string, YiDARequest> RequestMap { get; set; } = [];

    public class YiDAClientConfig
    {
        /// <summary>最大总尝试次数，包含第一次请求，至少为 1。</summary>
        public int RetryLimit { get; set; } = 3;
        public int RetryDelayMilliseconds { get; set; } = 500;
    }

    public class YiDARequest
    {
        public DefaultYiDARequest Request { get; set; } = new();
        public Dictionary<string, string> FormUuidMap { get; set; } = [];

        public DefaultYiDARequest GetByFormUuid(string formUuid)
        {
            if (!FormUuidMap.TryGetValue(formUuid, out var id))
                throw new YiDAException($"Unknown YiDA form alias: {formUuid}.");

            // 先克隆，再设置表单，避免并发请求修改配置中的共享模板。
            return Request.Clone().SetFormUuid(id);
        }
    }

    /// <summary>使用 application:form 选择应用；只有表单别名时使用第一个应用。</summary>
    public DefaultYiDARequest GetByFormUuid(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        string[] parts = key.Split(':');
        if (parts.Length is < 1 or > 2 || parts.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Use a form alias or application:form.", nameof(key));

        if (RequestMap.Count == 0)
            throw new YiDAException("YiDAConfig.RequestMap is empty.");

        if (parts.Length == 1)
            return RequestMap.Values.First().GetByFormUuid(parts[0]);

        if (!RequestMap.TryGetValue(parts[0], out var request))
            throw new YiDAException($"Unknown YiDA application alias: {parts[0]}.");

        return request.GetByFormUuid(parts[1]);
    }
}
