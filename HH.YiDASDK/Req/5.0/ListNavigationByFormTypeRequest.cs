using System;

namespace HH.YiDASDK;

/// <summary>
/// 5.3 获取应用下的页面列表
/// </summary>
public class ListNavigationByFormTypeRequest : BaseYiDARequest<ListNavigationByFormTypeResponse>
{
    /// <summary>
    /// 宜搭 具体请求方法
    /// </summary>
    /// <returns></returns>
    public override string GetUrl() => "/yida_vpc/app/listNavigationByFormType.json";

    /// <summary>
    /// 页面类型 receipt,单据页面process,流程页面report，报表页面
    /// </summary>
    public string formType { get; private set; }

    /// <summary>
    /// 页面类型
    /// </summary>
    /// <param name="obj">流程code</param>
    public void SetFormType(string formType) { this.formType = formType?.Trim(); }

    /// <summary>
    /// 数据验证
    /// </summary>
    public override void Validate()
    {
        formType.ValidateRequired("formType");

        base.Validate();
    }
}
