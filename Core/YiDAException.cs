using System.Diagnostics.CodeAnalysis;

namespace HH.YiDASDK.Core;

public class YiDAException
    : Exception
{
    /// <summary>宜搭返回的业务错误，包括原始响应正文。</summary>
    public YiDAResponse? Response { get; init; }
    public System.Net.HttpStatusCode? StatusCode { get; init; }

    public YiDAException() { }

    public YiDAException(string message)
        : base(message) { }

    public YiDAException(string message, Exception innerException)
        : base(message, innerException) { }

    [DoesNotReturn]
    public static void Throw(string message)
        => throw new YiDAException(message);
    [DoesNotReturn]
    public static void Throw(Exception innerException, string message)
        => throw new YiDAException(message, innerException);

    public static void ThrowIfNull(object? obj, string message)
        => ThrowIfTrue(obj is null, message);
    public static void ThrowIfTrue(Func<bool> func, string message)
        => ThrowIfTrue(func(), message);
    public static void ThrowIfTrue(bool flag, string message)
    {
        if (flag) Throw(message);
    }
}