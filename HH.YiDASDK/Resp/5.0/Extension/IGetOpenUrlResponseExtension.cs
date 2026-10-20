using Flurl.Http;
using HH.YiDASDK.Core;
using System.Net.Http;

namespace HH.YiDASDK;

/// <summary>
/// 5.1 附件地址转临时免登地址 扩展方法 接口
/// </summary>
public interface IGetOpenUrlResponseExtension : IYiDAResponse<GetOpenUrlResponse>
{
    /// <summary>
    /// 下载附件流，由调用方负责释放
    /// </summary>
    /// <param name="cancellationToken">取消下载</param>
    /// <returns>附件内容流</returns>
    Task<Stream> DownloadStreamAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 下载附件到本地目录，自动创建目录，覆盖同名文件
    /// </summary>
    /// <param name="localFolderPath">本地目录</param>
    /// <param name="localFileName">文件名，建议使用附件字段中的 name</param>
    /// <param name="cancellationToken">取消下载</param>
    /// <returns>下载文件的本地路径</returns>
    Task<string> DownloadFileAsync(string localFolderPath, string localFileName, CancellationToken cancellationToken = default);
}

/// <summary>
/// 5.1 附件地址转临时免登地址 扩展方法 实现
/// </summary>
public partial class GetOpenUrlResponse : IGetOpenUrlResponseExtension
{
    /// <summary>
    /// 下载附件流，由调用方负责释放
    /// </summary>
    /// <param name="cancellationToken">取消下载</param>
    /// <returns>附件内容流</returns>
    public Task<Stream> DownloadStreamAsync(CancellationToken cancellationToken = default)
    {
        ValidateDownload();
        cancellationToken.ThrowIfCancellationRequested();
        // 临时地址自带鉴权，直接 GET，不附加宜搭网关的签名请求头。
        return result.GetStreamAsync(cancellationToken, HttpCompletionOption.ResponseHeadersRead);
    }

    /// <summary>
    /// 下载附件到本地目录，自动创建目录，覆盖同名文件
    /// </summary>
    /// <param name="localFolderPath">本地目录</param>
    /// <param name="localFileName">文件名，建议使用附件字段中的 name</param>
    /// <param name="cancellationToken">取消下载</param>
    /// <returns>下载文件的本地路径</returns>
    public Task<string> DownloadFileAsync(string localFolderPath, string localFileName, CancellationToken cancellationToken = default)
    {
        ValidateDownload();
        localFolderPath.ValidateRequired("localFolderPath");
        localFileName.ValidateRequired("localFileName");
        cancellationToken.ThrowIfCancellationRequested();
        return result.DownloadFileAsync(localFolderPath, localFileName, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 下载前验证临时地址
    /// </summary>
    private void ValidateDownload()
    {
        YiDAException.ThrowIfTrue(!success, $"get open url failed: {ErrorInfo}");
        result.ValidateRequired("result");
    }
}
