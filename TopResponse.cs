namespace HH.YiDASDK;

/// <summary>
/// TOP响应基础对象。
/// </summary>
public abstract class TopResponse : TopObject
{
    /// <summary>
    /// 请求是否成功
    /// </summary>
    public bool success { get; set; }
    /// <summary>
    /// 实例 ID
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string body { get; set; } = string.Empty;
    /// <summary>
    /// 错误码
    /// </summary>
    public string? errorCode { get; set; }
    /// <summary>
    /// 错误信息
    /// </summary>
    public string? errorMsg { get; set; }
    /// <summary>
    /// 错误信息-其他错误
    /// </summary>
    public string? Message { get; set; }
    /// <summary>
    /// 错误信息 - 拼接
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string ErrorInfo
    {
        get
        {
            return $"{Message} {errorMsg}";
        }
    }

    public override string ToString()
    {
        return $"{{{nameof(success)}={success.ToString()}, {nameof(body)}={body}, {nameof(errorCode)}={errorCode}, {nameof(errorMsg)}={errorMsg}, {nameof(Message)}={Message}, {nameof(ErrorInfo)}={ErrorInfo}}}";
    }
}