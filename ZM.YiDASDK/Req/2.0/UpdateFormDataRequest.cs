using System.Collections.Generic;

namespace ZM.YiDASDK
{
    /// <summary>
    /// 2.2 更新表单中指定组件值
    /// </summary>
    public class UpdateFormDataRequest : BaseYiDARequest<UpdateFormDataResponse>
    {
        /// <summary>
        /// 宜搭 具体请求方法
        /// </summary>
        /// <returns></returns>
        public override string GetUrl() => "/yida_vpc/form/updateFormData.json";

        /// <summary>
        /// 表单数据
        /// </summary>
        public string updateFormDataJson { get; private set; }
        /// <summary>
        /// 使用最新的表单版本进行更新 默认false
        /// </summary>
        public bool useLatestVersion { get; private set; }

        /// <summary>
        /// 要更新的实例的实例ID
        /// </summary>
        public string formInstId { get; private set; }
        /// <summary>
        /// 设置实例ID
        /// </summary>
        /// <param name="instanceId">实例ID</param>
        public void SetFormInstId(string instanceId) { formInstId = instanceId?.Trim(); }

        /// <summary>
        /// 设置 表单数据
        /// </summary>
        /// <param name="str">字符串</param>
        public void SetUpdateFormDataJson(string str) { this.updateFormDataJson = str; }
        /// <summary>
        /// 设置 表单数据 对象序列化
        /// </summary>
        /// <param name="obj">object</param>
        public void SetUpdateFormDataJson(object obj) { this.updateFormDataJson = obj.Serialize(); }
        /// <summary>
        /// 设置 表单数据 对象转字典后序列化
        /// </summary>
        /// <param name="obj">object</param>
        public void SetUpdateFormData(object obj) { this.updateFormDataJson = obj?.ToDictionaryOfObjectValue()?.Serialize(); }
        /// <summary>
        /// 设置 表单数据 字典序列化
        /// </summary>
        /// <param name="dictionary">字典对象</param>
        public void SetUpdateFormData(IDictionary<string, string> dictionary) { this.updateFormDataJson = dictionary?.Serialize(); }
        /// <summary>
        /// 设置 表单数据 字典序列化
        /// </summary>
        /// <param name="dictionary">字典对象</param>
        public void SetUpdateFormData(IDictionary<string, object> dictionary) { this.updateFormDataJson = dictionary?.Serialize(); }

        /// <summary>
        /// 使用最新的表单版本进行更新
        /// </summary>
        public void SetUseLatestVersion() { this.useLatestVersion = true; }
        /// <summary>
        /// 数据验证
        /// </summary>
        public override void Validate()
        {
            formInstId.ValidateRequired("formInstId");
            updateFormDataJson.ValidateRequired("updateFormDataJson");

            base.Validate();
        }
    }
}
