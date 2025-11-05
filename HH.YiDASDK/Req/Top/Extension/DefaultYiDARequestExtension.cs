using HH.YiDASDK.Core;
using System.ComponentModel;

namespace HH.YiDASDK;

/// <summary>
/// 默认类型扩展
/// </summary>
public static class DefaultYiDARequestExtension
{
    /// <summary>
    /// 基础类型转换
    /// </summary>
    /// <typeparam name="D">转换目标类型</typeparam>
    /// <param name="req">基础载体</param>
    /// <returns>目标对象</returns>
    public static D ConvertTo<D>(this DefaultYiDARequest req) => req.Serialize().Deserialize<D>();
    /// <summary>
    /// 基础类型转换
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="config"></param>
    /// <returns></returns>
    public static D Get<D>(this YiDAConfig config)
    {
        Type type = typeof(D).GetGenericArguments()[0];
        DescriptionAttribute[] descriptions = (DescriptionAttribute[])type
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return config.GetByFormUuid(descriptions[0].Description).Serialize().Deserialize<D>();
    }
    /// <summary>
    /// 新增修改删除用
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="config"></param>
    /// <param name="m"></param>
    /// <returns></returns>
    /// <exception cref="AggregateException"></exception>
    public static D Get<D>(this YiDAConfig config, Type m)
    {
        DescriptionAttribute[] descriptions = (DescriptionAttribute[])m
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return config.GetByFormUuid(descriptions[0].Description).Serialize().Deserialize<D>();
    }
    /// <summary>
    /// 指定 name
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="config"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="AggregateException"></exception>
    public static D Get<D>(this YiDAConfig config, string name)
    {
        return config.GetByFormUuid(name).Serialize().Deserialize<D>();
    }
}
