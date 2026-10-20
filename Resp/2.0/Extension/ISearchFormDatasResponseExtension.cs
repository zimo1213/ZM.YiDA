using HH.YiDASDK.Core;

namespace HH.YiDASDK;

public static class ISearchFormDatasResponseExtension
{
    public static async Task<List<GetFormDataByIdResultDomain<D>>> FetchAllAsync<D>(
        this IYiDAClient yiDAClient, SearchFormDatasRequest<D> request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(yiDAClient);
        ArgumentNullException.ThrowIfNull(request);
        List<GetFormDataByIdResultDomain<D>> results = [];
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var response = await yiDAClient.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
            if (!response.success)
                throw new YiDAException($"YiDA query failed: {response.ErrorInfo}") { Response = response };
            var page = response.result ?? throw new YiDAException("YiDA query returned no page result.");
            if (page.data is not { Count: > 0 })
                return results;

            results.AddRange(page.data);
            var maxPageNumber = (int)Math.Ceiling(page.totalCount / request.pageSize);
            if (page.totalCount == 0 || request.currentPage >= maxPageNumber)
                return results;
            request.SetCurrentPage(request.currentPage + 1);
        }
    }
}
