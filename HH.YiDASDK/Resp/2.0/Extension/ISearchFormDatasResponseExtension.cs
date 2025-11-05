namespace HH.YiDASDK;

public static class ISearchFormDatasResponseExtension
{
    public static async Task<List<GetFormDataByIdResultDomain<D>>> FetchAllAsync<D>(this IYiDAClient yiDAClient, SearchFormDatasRequest<D> request)
    {
        SearchFormDatasResponse<D> rsp = await yiDAClient.ExecuteAsync(request);
        int totalCount = rsp.result.totalCount;
        int maxPageNumber = (int)Math.Ceiling(totalCount / request.pageSize);

        List<GetFormDataByIdResultDomain<D>> result = [];
        while (rsp.success && rsp.result.data is not null && rsp.result.data.Any())
        {
            result.AddRange(rsp.result.data);
            request.SetCurrentPage(rsp.result.currentPage + 1);
            if (rsp.result.totalCount == 0 || request.currentPage > maxPageNumber)
            {
                break;
            }
            rsp = await yiDAClient.ExecuteAsync(request);
        }
        return result;
    }
}