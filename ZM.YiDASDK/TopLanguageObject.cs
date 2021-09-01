namespace ZM.YiDASDK
{
    /// <summary>
    /// TOP基础语言对象。
    /// </summary>
    public abstract class TopLanguageObject : TopObject
    {
        /// <summary>
        /// 未知
        /// </summary>
        public string pureEn_US { get; set; }
        /// <summary>
        /// 英文
        /// </summary>
        public string en_US { get; set; }
        /// <summary>
        /// 中文
        /// </summary>
        public string zh_CN { get; set; }
        /// <summary>
        /// i18n
        /// </summary>
        public string type { get; set; }
    }
}
