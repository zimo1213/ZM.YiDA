namespace HH.YiDASDK;

/// <summary>
/// TOP客户端异常。
/// </summary>
public class TopException : Exception
{
    /// <summary>
    /// 空构造
    /// </summary>
    public TopException()
        : base()
    {
    }
    /// <summary>
    /// TOP客户端异常 - 已知异常
    /// </summary>
    /// <param name="message">提示信息</param>
    public TopException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// TOP客户端异常 - 已知异常加异常对象
    /// </summary>
    /// <param name="message">提示信息</param>
    public TopException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// TOP客户端异常 错误码加错误信息
    /// </summary>
    /// <param name="errorCode">错误码</param>
    /// <param name="errorMsg">错误信息</param>
    public TopException(string errorCode, string errorMsg)
        : base($"{errorCode}:{errorMsg}")
    {
    }

}
