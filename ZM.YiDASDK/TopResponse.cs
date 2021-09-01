namespace ZM.YiDASDK
{
    /// <summary>
    /// TOP响应基础对象。
    /// </summary>
    public abstract class TopResponse : TopObject
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 实例 ID
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errorMsg { get; set; }
    }
}