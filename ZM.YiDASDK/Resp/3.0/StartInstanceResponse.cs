namespace ZM.YiDASDK
{
    /// <summary>
    /// 3.1 发起新的流程实例
    /// </summary>
    public partial class StartInstanceResponse : YiDAResponse
    {
        /// <summary>
        /// 新增 流程ID
        /// </summary>
        public string result { get; set; }
    }
}
