namespace HH.YiDASDK;

/// <summary>
/// 5.1 附件地址转临时免登地址
/// </summary>
public partial class GetOpenUrlResponse : YiDAResponse
{
    /// <summary>
    /// 临时免登地址，有效期由请求的 timeout 指定，取得后应及时下载
    /// </summary>
    public string result { get; set; } = string.Empty;
}
