using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ZM.YiDASDK
{
    /// <summary>
    /// 2.6 根据条件搜索表单实例详情列表
    /// </summary>
    public class SearchFormDatasRequest : SearchFormDatasRequest<IDictionary<string, object>> { }

    /// <summary>
    /// 2.6 根据条件搜索表单实例详情列表
    /// </summary>
    public class SearchFormDatasRequest<D> : BasePageYiDARequest<SearchFormDatasResponse<D>>
    {
        /// <summary>
        /// 宜搭 具体请求方法
        /// </summary>
        /// <returns></returns>
        public override string GetUrl() => "/yida_vpc/form/searchFormDatas.json";

        /// <summary>
        /// 指定排序字段
        /// </summary>
        public string dynamicOrder { get; private set; }

        /// <summary>
        /// 根据表单内组件值查询
        /// </summary>
        /// <param name="obj"></param>
        public void SetDynamicOrder(object obj) { this.dynamicOrder = obj?.ToDictionaryOfObjectValue()?.Serialize(); }
        /// <summary>
        /// 根据表单内组件值查询
        /// </summary>
        /// <param name="dictionary"></param>
        public void SetDynamicOrder(IDictionary<string, string> dictionary) { this.dynamicOrder = dictionary?.Serialize(); }
        /// <summary>
        /// 根据表单内组件值查询
        /// </summary>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否倒序</param>
        public void SetDynamicOrder(string order, bool desc = false)
        {
            this.dynamicOrder = new Dictionary<string, string> { { order, desc ? "-" : "+" } }.Serialize();
        }
        /// <summary>
        /// 倒序 查询
        /// </summary>
        /// <param name="order">排序字段</param>
        /// <param name="desc">是否倒序</param>
        public void SetOrderByDescending(string order)
        {
            SetDynamicOrder(order, true);
        }
        /// <summary>
        /// 正序 查询
        /// </summary>
        /// <param name="order">排序字段</param>
        public void SetOrderBy(string order)
        {
            SetDynamicOrder(order, false);
        }

        /// <summary>
        /// 倒序 查询
        /// </summary>
        /// <param name="order">排序字段</param>
        public void OrderByDescending(Expression<Func<D, string>> keySelector)
        {
            SetDynamicOrder((keySelector.Body as MemberExpression).Member.Name, true);
        }
        /// <summary>
        /// 正序 查询
        /// </summary>
        /// <param name="order">排序字段</param>
        public void OrderBy(Expression<Func<D, string>> keySelector)
        {
            SetDynamicOrder((keySelector.Body as MemberExpression).Member.Name, false);
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        public override void Validate()
        {
            base.Validate();
        }
    }
}
