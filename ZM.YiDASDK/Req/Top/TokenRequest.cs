namespace ZM.YiDASDK
{
    /// <summary>
    /// Token 请求实体
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// 开发账户的 key 
        /// </summary>
        public string key { get; private set; }
        /// <summary>
        /// 开发账户的 secret 
        /// </summary>
        public string secret { get; private set; }
        /// <summary>
        /// 初始化构造
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="secret">secret</param>
        public TokenRequest(string key, string secret)
        {
            this.key = key;
            this.secret = secret;
        }
        /// <summary>
        /// 设置 key 
        /// </summary>
        /// <param name="key"></param>
        public void SetKey(string key) { this.key = key?.Trim(); }
        /// <summary>
        /// 设置 secret 
        /// </summary>
        /// <param name="secret"></param>
        public void SetSecret(string secret) { this.secret = secret?.Trim(); }
    }
}
