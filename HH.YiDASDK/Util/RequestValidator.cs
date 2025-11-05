namespace HH.YiDASDK;

/// <summary>
/// 请求验证器。
/// </summary>
internal static class RequestValidator
{
    private const string ERR_CODE_PARAM_MISSING = "40";
    private const string ERR_CODE_PARAM_INVALID = "41";
    private const string ERR_MSG_PARAM_MISSING = "client-error:Missing required arguments:{0}";
    private const string ERR_MSG_PARAM_INVALID_STRING = "client-error:Invalid arguments string :{0}";
    /// <summary>
    /// 验证是否为 null
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="name">参数名</param>
    private static void CheckNull(this object value, string name)
    {
        if (value is null) throw new TopException(ERR_CODE_PARAM_MISSING, string.Format(ERR_MSG_PARAM_MISSING, name));
    }
    /// <summary>
    /// 验证参数是否为空。
    /// </summary>
    /// <param name="value">参数值</param>
    /// <param name="name">参数名</param>
    public static void ValidateRequired(this string value, string name)
    {
        value.CheckNull(name);
        if (string.IsNullOrWhiteSpace(value)) throw new TopException(ERR_CODE_PARAM_INVALID, string.Format(ERR_MSG_PARAM_INVALID_STRING, name));
    }
}
