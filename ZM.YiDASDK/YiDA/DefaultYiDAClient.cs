using System.Collections.Generic;

namespace ZM.YiDASDK
{
    /// <summary>
    ///  SDK实现
    /// </summary>
    public class DefaultYiDAClient : IYiDAClient
    {
        private const string BASE_URL = "https://s-api.alibaba-inc.com";
        private const string VERSION = "1.0";
        private const string NONCE = "1213927";
        private const string DEFAULT_KEY = "shanghaihonghuan-6FVMUlPrmyyQj";
        private const string DEFAULT_SECRET = "y20ORWGs734DC3m2Npjhp80eM4CR07hLGk3J7hf7";

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <typeparam name="T">基础返回抽象对象</typeparam>
        /// <param name="request">请求对象</param>
        /// <param name="token">身份对象</param>
        /// <returns></returns>
        public T Execute<T>(IYiDARequest<T> request, TokenRequest token) where T : YiDAResponse
        {
            request.Validate();
            var serverUrl = request.GetUrl();
            var parms = request.ToDictionaryOfStringValue();
            var header = GetHeader(parms, serverUrl, token ?? new TokenRequest(DEFAULT_KEY, DEFAULT_SECRET));
            var result = WebUtils.Post($"{BASE_URL}{serverUrl}", parms, header);
            T rsp = result.Deserialize<T>();
            rsp.body = result;
            return rsp;
        }

        /// <summary>
        /// 获取请求头
        /// </summary>
        /// <param name="parms">请求参数</param>
        /// <param name="serverUrl">请求网址</param>
        /// <param name="token">身份对象</param>
        /// <returns></returns>
        private IDictionary<string, string> GetHeader(IDictionary<string, string> parms, string serverUrl, TokenRequest token)
        {
            var timestamp = YiDASignatureUtil.Iso8601Date();
            var sign = YiDASignatureUtil.Signature(parms, timestamp, NONCE, serverUrl, token.secret);
            return new Dictionary<string, string>
            {
                 {  "X-Hmac-Auth-Timestamp",timestamp },
                 {  "X-Hmac-Auth-Version" ,VERSION },
                 {  "X-Hmac-Auth-Nonce" ,NONCE },
                 {  "X-Hmac-Auth-Signature" ,sign },
                 {  "apiKey",token.key }
            };
        }
    }
}
