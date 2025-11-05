namespace HH.YiDASDK;

/// <summary>
/// 基础请求对象
/// </summary>
public class DefaultYiDARequest
{
    /// <summary>
    /// 应用ID
    /// </summary>
    public string appType { get; set; }
    /// <summary>
    /// 表单ID
    /// </summary>
    public string formUuid { get; set; }
    /// <summary>
    /// 语言 默认中文 , 英文 请调用 SetEnglishLanguage
    /// </summary>
    public string language { get; set; }
    /// <summary>
    /// 应用秘钥
    /// </summary>
    public string systemToken { get; set; }
    /// <summary>
    /// 钉钉的userId
    /// </summary>
    public string userId { get; set; }
    /// <summary>
    /// 空构造 默认中文
    /// </summary>
    public DefaultYiDARequest()
    {
        language = "zh_CN";
    }
    /// <summary>
    /// 默认构造 
    /// </summary>
    /// <param name="appType"></param>
    /// <param name="systemToken"></param>
    public DefaultYiDARequest(string appType, string systemToken) : this()
    {
        SetAppType(appType);
        SetSystemToken(systemToken);
    }
    /// <summary>
    /// 默认构造
    /// </summary>
    /// <param name="appType"></param>
    /// <param name="systemToken"></param>
    /// <param name="userId"></param>
    public DefaultYiDARequest(string appType, string systemToken, string userId) : this(appType, systemToken)
    {
        SetUserId(userId);
    }
    /// <summary>
    /// 默认构造
    /// </summary>
    /// <param name="appType"></param>
    /// <param name="systemToken"></param>
    /// <param name="userId"></param>
    /// <param name="formUuid"></param>
    public DefaultYiDARequest(string appType, string systemToken, string userId, string formUuid) : this(appType, systemToken, userId)
    {
        SetFormUuid(formUuid);
    }
    /// <summary>
    /// 参数验证
    /// </summary>
    public virtual void Validate()
    {
        appType.ValidateRequired("appType");
        formUuid.ValidateRequired("formUuid");
        systemToken.ValidateRequired("systemToken");
        userId.ValidateRequired("userId");
    }
    /// <summary>
    /// 设置 应用编码
    /// </summary>
    /// <param name="appType">应用编码</param>
    public DefaultYiDARequest SetAppType(string appType) { this.appType = appType?.Trim(); return Clone(); }
    /// <summary>
    /// 设置 应用密钥
    /// </summary>
    /// <param name="systemToken">应用密钥</param>
    public DefaultYiDARequest SetSystemToken(string systemToken) { this.systemToken = systemToken?.Trim(); return Clone(); }
    /// <summary>
    /// 设置 表单ID
    /// </summary>
    /// <param name="formUuid">表单ID</param>
    /// <returns></returns>
    public DefaultYiDARequest SetFormUuid(string formUuid) { this.formUuid = formUuid?.Trim(); return Clone(); }
    /// <summary>
    /// 设置 请求用户ID
    /// </summary>
    /// <param name="userId">请求用户ID</param>
    /// <returns></returns>
    public DefaultYiDARequest SetUserId(string userId) { this.userId = userId?.Trim(); return Clone(); }
    /// <summary>
    /// 设置英文语言
    /// </summary>
    /// <returns></returns>
    public DefaultYiDARequest SetEnglishLanguage() { this.language = "en_US"; return Clone(); }

    /// <summary>
    /// 克隆对象
    /// </summary>
    /// <returns></returns>
    public DefaultYiDARequest Clone() => (DefaultYiDARequest)MemberwiseClone();
}
