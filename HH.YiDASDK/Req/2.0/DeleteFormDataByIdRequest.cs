namespace HH.YiDASDK;

/// <summary>
/// 2.3 删除表单实例
/// </summary>
public partial class DeleteFormDataByIdRequest<D> : DeleteFormDataByIdRequest where D : class;
public partial class DeleteFormDataByIdRequest : BaseYiDARequest<DeleteFormDataByIdResponse>
{
    /// <summary>
    /// 宜搭 具体请求方法
    /// </summary>
    /// <returns></returns>
    public override string GetUrl() => "/yida_vpc/form/deleteFormData.json";

    /// <summary>
    /// 要删除的实例的实例ID
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
