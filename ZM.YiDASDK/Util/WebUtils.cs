using RestSharp;
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
            IRestClient client = new RestClient(url) { Timeout = -1 };
            IRestRequest request = new RestRequest(Method.POST);
            request.AddHeaders(header);
            request.AddParameter("x-www-form-urlencoded", parms.GetKeyValuePairsString(), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
