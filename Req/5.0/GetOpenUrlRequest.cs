namespace HH.YiDASDK;

/// <summary>
/// 5.1 附件地址转临时免登地址
/// </summary>
public class GetOpenUrlRequest : BaseYiDARequest<GetOpenUrlResponse>
{
    /// <summary>
    /// 宜搭 具体请求方法
    /// </summary>
    /// <returns></returns>
    public override string GetUrl() => "/yida_vpc/file/getOpenUrl.json";

    /// <summary>
    /// 宜搭附件地址
    /// </summary>
    [System.Text.Json.Serialization.JsonInclude]
    public string fileUrl { get; private set; } = string.Empty;
    /// <summary>
    /// 临时地址有效期，单位毫秒，默认一分钟
    /// </summary>
    [System.Text.Json.Serialization.JsonInclude]
    public int timeout { get; private set; } = 60000;

    /// <summary>
    /// 设置 附件地址，支持完整地址或以 / 开头的宜搭相对地址
    /// </summary>
    /// <param name="fileUrl">附件字段中的 downloadUrl 或 url</param>
    public void SetFileUrl(string fileUrl)
    {
        this.fileUrl = fileUrl?.Trim() ?? string.Empty;
        if (this.fileUrl.StartsWith("/", StringComparison.Ordinal))
        {
            this.fileUrl = "https://www.aliwork.com" + this.fileUrl;
        }
    }
    /// <summary>
    /// 设置 临时地址有效期
    /// </summary>
    /// <param name="timeout">毫秒，必须大于零</param>
    public void SetTimeout(int timeout) { this.timeout = timeout; }

    /// <summary>
    /// 数据验证
    /// </summary>
    public override void Validate()
    {
        // 附件接口为应用级接口，不需要基础验证中的 formUuid。
        appType.ValidateRequired("appType");
        systemToken.ValidateRequired("systemToken");
        userId.ValidateRequired("userId");
        fileUrl.ValidateRequired("fileUrl");

        if (!Uri.TryCreate(fileUrl, UriKind.Absolute, out Uri? uri)
            || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp))
        {
            throw new TopException("41", "client-error:Invalid arguments string :fileUrl");
        }
        if (timeout <= 0)
        {
            throw new TopException("41", "client-error:Invalid arguments :timeout must be greater than zero");
        }
    }
}
