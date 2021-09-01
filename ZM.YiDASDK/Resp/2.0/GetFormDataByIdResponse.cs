namespace ZM.YiDASDK
{
    /// <summary>
    /// 2.4 根据表单 ID 查询实例详情
    /// </summary>
    public partial class GetFormDataByIdResponse<D> : YiDAResponse
    {
        /// <summary>
        /// 实例详情 对象 
        /// </summary>
        public GetFormDataByIdResultDomain<D> result { get; set; }
    }

    /// <summary>
    /// 实例详情 对象 
    /// </summary>
    public class GetFormDataByIdResultDomain<D> : TopObject
    {
        /// <summary>
        /// 表单创建时间
        /// </summary>
        public string gmtCreate { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public string gmtModified { get; set; }
        /// <summary>
        /// 发起人
        /// </summary>
        public string creator { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        public string modifier { get; set; }
        /// <summary>
        /// 发起人 详情
        /// </summary>
        public GetFormDataByIdResultOriginatorDomain originator { get; set; }
        /// <summary>
        /// 修改用户 详情
        /// </summary>
        public GetFormDataByIdResultModifyUserDomain modifyUser { get; set; }
        /// <summary>
        /// 实例 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        ///  表单 id
        /// </summary>
        public string modelUuid { get; set; }
        /// <summary>
        /// 表单版本
        /// </summary>
        public int version { get; set; }
        /// <summary>
        /// 未知
        /// </summary>
        public string instValue { get; set; }
        /// <summary>
        /// 可能是 实例 id
        /// </summary>
        public string formInstId { get; set; }
        /// <summary>
        /// 表单数据详情 字典
        /// </summary>
        public D formData { get; set; }
    }

    /// <summary>
    /// 发起人 对象
    /// </summary>
    public class GetFormDataByIdResultOriginatorDomain : TopObject
    {
        /// <summary>
        /// 发起人 名称 对象
        /// </summary>
        public GetFormDataByIdResultOriginatorNameDomain name { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }
    }
    /// <summary>
    /// 发起人 名称 对象
    /// </summary>
    public class GetFormDataByIdResultOriginatorNameDomain : TopLanguageObject
    {
    }
    /// <summary>
    /// 修改 用户 对象
    /// </summary>
    public class GetFormDataByIdResultModifyUserDomain : TopObject
    {
        /// <summary>
        /// 修改 用户 名称 对象
        /// </summary>
        public GetFormDataByIdResultModifyUserNameDomain name { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }
    }
    /// <summary>
    /// 修改 用户 名称 对象
    /// </summary>
    public class GetFormDataByIdResultModifyUserNameDomain : TopLanguageObject
    {
    }
}
