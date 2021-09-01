using System.Collections.Generic;

namespace ZM.YiDASDK
{
    /// <summary>
    /// 3.1 发起新的流程实例
    /// </summary>
    public class StartInstanceRequest : BaseYiDARequest<StartInstanceResponse>
    {
        /// <summary>
        /// 宜搭 具体请求方法
        /// </summary>
        /// <returns></returns>
        public override string GetUrl() => "/yida_vpc/process/startInstance.json";

        /// <summary>
        /// 流程code 单独发起页链接上可查
        /// </summary>
        public string processCode { get; private set; }
        /// <summary>
        /// 表单数据
        /// </summary>
        public string formDataJson { get; private set; }
        /// <summary>
        /// 发起人所在部门号
        /// </summary>
        public string deptId { get; private set; }

        /// <summary>
        /// 设置 流程code
        /// </summary>
        /// <param name="obj">流程code</param>
        public void SetProcessCode(string processCode) { this.processCode = processCode?.Trim(); }
        /// <summary>
        /// 设置 发起人所在部门号
        /// </summary>
        /// <param name="deptId">发起人所在部门号</param>
        public void SetDeptId(string deptId) { this.deptId = deptId?.Trim(); }


        /// <summary>
        /// 设置 表单数据
        /// </summary>
        /// <param name="str">字符串</param>
        public void SetFormDataJson(string str) { this.formDataJson = str; }
        /// <summary>
        /// 设置 表单数据 对象序列化
        /// </summary>
        /// <param name="obj">object</param>
        public void SetFormDataJson(object obj) { this.formDataJson = obj.Serialize(); }
        /// <summary>
        /// 设置 表单数据 对象转字典后序列化
        /// </summary>
        /// <param name="obj">object</param>
        public void SetFormData(object obj) { this.formDataJson = obj?.ToDictionaryOfObjectValue()?.Serialize(); }
        /// <summary>
        /// 设置 表单数据 字典序列化
        /// </summary>
        /// <param name="dictionary">字典对象</param>
        public void SetFormData(IDictionary<string, string> dictionary) { this.formDataJson = dictionary?.Serialize(); }
        /// <summary>
        /// 设置 表单数据 字典序列化
        /// </summary>
        /// <param name="dictionary">字典对象</param>
        public void SetFormData(IDictionary<string, object> dictionary) { this.formDataJson = dictionary?.Serialize(); }

        /// <summary>
        /// 数据验证
        /// </summary>
        public override void Validate()
        {
            processCode.ValidateRequired("processCode");
            formDataJson.ValidateRequired("formDataJson");

            base.Validate();
        }
    }
}
