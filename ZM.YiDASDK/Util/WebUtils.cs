using Flurl.Http;
using System.Collections.Generic;

namespace ZM.YiDASDK
{
    /// <summary>
    /// 第三方请求扩展
    /// </summary>
    internal class WebUtils
    {
        /// <summary>
        /// POST
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parms">请求参数</param>
        /// <param name="header">请求头</param>
        /// <returns>响应Content</returns>
        public static string Post(string url, IDictionary<string, string> parms, IDictionary<string, string> header)
        {
            FlurlRequest req = new FlurlRequest(url);
            req.WithHeaders(header);
            System.Threading.Tasks.Task<IFlurlResponse> res = req.PostUrlEncodedAsync(parms);
            res.Wait();
            System.Threading.Tasks.Task<string> str = res.Result.GetStringAsync();
            str.Wait();
            return str.Result;
        }
    }
}
