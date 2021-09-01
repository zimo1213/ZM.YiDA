using System.Collections.Generic;

namespace ZM.YiDASDK
{
    /// <summary>
    /// 2.4 根据表单 ID 查询实例详情
    /// </summary>
    public partial class GetFormDataByIdRequest : GetFormDataByIdRequest<IDictionary<string, object>> { }


    /// <summary>
    /// 2.4 根据表单 ID 查询实例详情
    /// </summary>
    public partial class GetFormDataByIdRequest<D> : BaseYiDARequest<GetFormDataByIdResponse<D>>
    {
        /// <summary>
        /// 宜搭 具体请求方法
        /// </summary>
        /// <returns></returns>
        public override string GetUrl() => "/yida_vpc/form/getFormDataById.json";

        /// <summary>
        /// 要查询的实例的实例ID
        /// </summary>
        public string formInstId { get; private set; }
        /// <summary>
        /// 设置实例ID
        /// </summary>
        /// <param name="instanceId">实例ID</param>
        public void SetFormInstId(string instanceId) { formInstId = instanceId?.Trim(); }

        /// <summary>
        /// 数据验证
        /// </summary>
        public override void Validate()
        {
            formInstId.ValidateRequired("formInstId");

            base.Validate();
        }
    }
}
