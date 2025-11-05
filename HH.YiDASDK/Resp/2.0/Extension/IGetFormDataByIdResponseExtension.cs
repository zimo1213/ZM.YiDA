using System.Text;

namespace HH.YiDASDK;


/// <summary>
/// 2.4 根据表单 ID 查询实例详情 扩展方法 接口
/// </summary>
public interface IGetFormDataByIdResponseExtension : IYiDAResponse<GetFormDataByIdResponse<IDictionary<string, object>>>
{
    /// <summary>
    /// 获取模板 - 简易模板 适用
    /// </summary>
    /// <returns></returns>
    string? GetTemplate();
}
/// <summary>
/// 2.4 根据表单 ID 查询实例详情 扩展方法 接口
/// </summary>
public partial class GetFormDataByIdResponse<D> : IGetFormDataByIdResponseExtension
{
    /// <summary>
    /// 获取模板 - 简易模板 适用
    /// </summary>
    /// <returns>字典代码块</returns>
    public string? GetTemplate()
    {
        if (!success || result is null) { return null; }

        if (result.formData is not IDictionary<string, object> dictionary) { return "仅仅支持默认类型, 指定D 后不能解析"; }

        KeyValuePair<string, object>[] d = dictionary.Reverse().ToArray();

        StringBuilder sb = new("IDictionary<string, string> dics = new Dictionary<string, string>{");
        foreach (KeyValuePair<string, object> t in d)
        {
            sb.Append("\n\t{ \"" + t.Key + "\",\"" + t.Value.Serialize()?.Replace("\\", "\\\\")?.Replace("\"", "\\\"") + "\" },// ");
        }
        sb.Append("\n};");
        return sb.ToString();
    }
}
