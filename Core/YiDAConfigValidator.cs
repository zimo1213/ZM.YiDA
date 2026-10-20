using Microsoft.Extensions.Options;

namespace HH.YiDASDK.Core;

internal sealed class YiDAConfigValidator : IValidateOptions<YiDAConfig>
{
    public ValidateOptionsResult Validate(string? name, YiDAConfig options)
    {
        List<string> failures = [];
        Require(options.Key, "Key");
        Require(options.Secret, "Secret");
        Require(options.Version, "Version");
        Require(options.Nonce, "Nonce");
        if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp)
            || uri.Query.Length != 0 || uri.Fragment.Length != 0 || uri.UserInfo.Length != 0)
            failures.Add("YiDAConfig.BaseUrl must be an absolute HTTP(S) URL without credentials, query or fragment.");
        if (options.ClientConfig is null || options.ClientConfig.RetryLimit < 1
            || options.ClientConfig.RetryDelayMilliseconds < 0)
            failures.Add("YiDAConfig.ClientConfig requires RetryLimit >= 1 and RetryDelayMilliseconds >= 0.");
        if (options.RequestMap is null)
        {
            failures.Add("YiDAConfig.RequestMap must not be null.");
        }
        else
        {
            foreach (var (alias, item) in options.RequestMap)
            {
                if (string.IsNullOrWhiteSpace(alias) || alias.Contains(':') || item?.Request is null)
                {
                    failures.Add("YiDAConfig.RequestMap requires nonempty application aliases and request templates.");
                    continue;
                }
                Require(item.Request.appType, $"RequestMap.{alias}.Request.appType");
                Require(item.Request.systemToken, $"RequestMap.{alias}.Request.systemToken");
                Require(item.Request.userId, $"RequestMap.{alias}.Request.userId");
                if (item.FormUuidMap is null || item.FormUuidMap.Any(pair =>
                    string.IsNullOrWhiteSpace(pair.Key) || pair.Key.Contains(':') || string.IsNullOrWhiteSpace(pair.Value)))
                    failures.Add($"YiDAConfig.RequestMap.{alias}.FormUuidMap contains invalid form mappings.");
            }
        }
        return failures.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(failures);

        void Require(string? value, string path)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Any(char.IsControl))
                failures.Add($"YiDAConfig.{path} must be nonempty and contain no control characters.");
        }
    }
}
