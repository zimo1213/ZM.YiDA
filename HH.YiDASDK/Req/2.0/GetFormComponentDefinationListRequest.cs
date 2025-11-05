namespace HH.YiDASDK;

/// <summary>
/// 2.7 获取表单定义
/// </summary>
public class GetFormComponentDefinationListRequest<D> : GetFormComponentDefinationListRequest where D : class;
public class GetFormComponentDefinationListRequest : BaseYiDARequest<GetFormComponentDefinationListResponse>
{
    /// <summary>
    /// 宜搭 具体请求方法
    /// </summary>
    /// <returns></returns>
    public override string GetUrl() => "/yida_vpc/formDesign/getFormComponentDefinationList.json";
}
