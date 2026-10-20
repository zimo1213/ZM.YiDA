using HH.YiDASDK.Core;

namespace HH.YiDASDK;

public interface IGetOpenUrlResponseExtension : IYiDAResponse<GetOpenUrlResponse>
{
    Task<Stream> DownloadStreamAsync(CancellationToken cancellationToken = default);
    Task<string> DownloadFileAsync(string localFolderPath, string localFileName, CancellationToken cancellationToken = default);
}

public partial class GetOpenUrlResponse : IGetOpenUrlResponseExtension
{
    private IHttpClientFactory? downloadClientFactory;

    internal void AttachDownloadClient(IHttpClientFactory factory) => downloadClientFactory = factory;

    /// <summary>流由调用方释放；释放时同时释放响应和工厂创建的客户端。</summary>
    public Task<Stream> DownloadStreamAsync(CancellationToken cancellationToken = default)
    {
        ValidateDownload();
        cancellationToken.ThrowIfCancellationRequested();
        if (downloadClientFactory is null)
            throw new YiDAException("Use ExecuteAsync to obtain the response, or pass an HttpClient to DownloadStreamAsync.");
        return OpenStreamAsync(downloadClientFactory.CreateClient(YiDAHttpClientNames.Download), true, cancellationToken);
    }

    /// <summary>供手动构造或反序列化的响应使用；客户端由调用方管理。</summary>
    public Task<Stream> DownloadStreamAsync(HttpClient client, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(client);
        ValidateDownload();
        cancellationToken.ThrowIfCancellationRequested();
        return OpenStreamAsync(client, false, cancellationToken);
    }

    public Task<string> DownloadFileAsync(string localFolderPath, string localFileName,
        CancellationToken cancellationToken = default)
        => SaveFileAsync(localFolderPath, localFileName, DownloadStreamAsync, cancellationToken);

    /// <summary>供手动构造的响应使用；自动创建目录并覆盖同名文件。</summary>
    public Task<string> DownloadFileAsync(HttpClient client, string localFolderPath, string localFileName,
        CancellationToken cancellationToken = default)
        => SaveFileAsync(localFolderPath, localFileName, token => DownloadStreamAsync(client, token), cancellationToken);

    private async Task<Stream> OpenStreamAsync(HttpClient client, bool ownsClient, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = null;
        try
        {
            // 附件临时地址自带鉴权，不附加网关签名头。
            using var request = new HttpRequestMessage(HttpMethod.Get, result);
            response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return new HttpResponseStream(stream, response, ownsClient ? client : null);
        }
        catch
        {
            response?.Dispose();
            if (ownsClient) client.Dispose();
            throw;
        }
    }

    private static async Task<string> SaveFileAsync(string folder, string fileName,
        Func<CancellationToken, Task<Stream>> openStream, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        if (Path.GetFileName(fileName) != fileName || fileName is "." or "..")
            throw new ArgumentException("Expected a file name without a directory.", nameof(fileName));
        cancellationToken.ThrowIfCancellationRequested();
        await using var stream = await openStream(cancellationToken).ConfigureAwait(false);
        Directory.CreateDirectory(folder);
        var path = Path.Combine(folder, fileName);
        await using var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None,
            bufferSize: 81920, options: FileOptions.Asynchronous | FileOptions.SequentialScan);
        await stream.CopyToAsync(file, cancellationToken).ConfigureAwait(false);
        return path;
    }

    private void ValidateDownload()
    {
        YiDAException.ThrowIfTrue(!success, $"Get open URL failed: {ErrorInfo}");
        result.ValidateRequired(nameof(result));
        if (!Uri.TryCreate(result, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            throw new YiDAException("The attachment URL must be an absolute HTTP(S) URL.");
    }
}
