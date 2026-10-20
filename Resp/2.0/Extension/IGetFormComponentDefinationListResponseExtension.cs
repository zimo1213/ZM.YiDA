using System.Text;

namespace HH.YiDASDK;

/// <summary>
/// 2.7 获取表单定义 扩展方法 接口
/// </summary>
public interface IGetFormComponentDefinationListResponseExtension : IYiDAResponse<GetFormComponentDefinationListResponse>
{
    /// <summary>
    /// 获取模板 - 转字典 - 简易单层
    /// </summary>
    /// <returns></returns>
    string? OutputTemplateOfSimpleDictionary();
    /// <summary>
    /// 获取模板 - 转匿名对象
    /// </summary>
    /// <returns></returns>
    string? OutputTemplateOfObject();
}
/// <summary>
/// 2.7 获取表单定义 扩展方法 实现
/// </summary>
public partial class GetFormComponentDefinationListResponse : IGetFormComponentDefinationListResponseExtension
{
    /// <summary>
    /// 获取模板 - 转匿名对象
    /// </summary>
    /// <returns></returns>
    public string? OutputTemplateOfObject()
    {
        if (!success || content is null || !content.Any()) { return null; }

        //查询 默认 FormContainer
        string? parentId = content.FirstOrDefault(f => f.componentName == "FormContainer")?.key;
        if (parentId is null) { return null; }
        //构建 first
        List<FormComponentDefinationListContentDomain> parent = content.Where(w => w.parentId == parentId).ToList();

        StringBuilder sb = new StringBuilder("var template = new {");
        foreach (FormComponentDefinationListContentDomain p in parent)
        {
            List<FormComponentDefinationListContentDomain> children = content.Where(w => w.parentId == p.key).ToList();
            if (children.Any())
            {
                sb.Append($"\n\t {p.key}=new object[] // {p.desc} | {p.componentName}");
                sb.Append("\n{\t\tnew {");
                foreach (FormComponentDefinationListContentDomain c in children)
                {
                    sb.Append($"\n\t\t\t {c.key}={GetDefaultValue(p.componentName)} ,// {c.desc} | {c.componentName}");
                }
                sb.Append("\n\t\t},");
                sb.Append("\n\t},");
            }
            else
            {
                sb.Append($"\n\t {p.key}={GetDefaultValue(p.componentName)} , // {p.desc} | {p.componentName}");
            }
        }
        sb.Append("\n};");
        return sb.ToString();
    }

    /// <summary>
    /// 获取控件对应输入格式
    /// </summary>
    /// <param name="componentName">控件类型</param>
    /// <returns></returns>
    private string GetDefaultValue(string componentName)
    {
        switch (componentName.ToLower())
        {
            case "checkboxfield": //单行输入框
            case "multiselectfield": //多行输入框
            case "cascadedate": // 
            case "employeefield":
            case "cityselectfield":
                return " new string[]{ \"\" }";

            case "cascadeselectfield":
                return " new string[]{ new {\"\"} }";

            case "attachmentfield":
                return " new {downloadUrl= \"\"  , name= \"\"  ,previewUrl= \"\"  ,url= \"\"  ,ext= \"\"  ,}";

            case "textfield":
            case "textareafield":
            case "numberfield":
            case "selectfield":
            case "datefield":
            case "departmentfield":
            default:
                return "\"\"";
        }
    }

    /// <summary>
    /// 获取模板
    /// </summary>
    /// <returns>字典代码块</returns>
    public string? OutputTemplateOfSimpleDictionary()
    {
        if (!success || content is null || !content.Any()) { return null; }

        FormComponentDefinationListContentDomain[] d = content.Where(w => w.parentId is not null && !string.IsNullOrWhiteSpace(w.key))
            .Reverse()
            .ToArray();

        StringBuilder sb = new("IDictionary<string, string> dics = new Dictionary<string, string>{");
        foreach (FormComponentDefinationListContentDomain t in d)
        {
            sb.Append("\n\t{ \"" + t.key + "\",\"\" },// " + t.desc + " | " + t.componentName);
        }
        sb.Append("\n};");
        return sb.ToString();
    }
}
