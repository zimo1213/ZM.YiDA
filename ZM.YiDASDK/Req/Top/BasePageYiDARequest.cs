using System;
using System.Collections.Generic;

namespace ZM.YiDASDK
{
    /// <summary>
    /// 基础分页请求抽象对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BasePageYiDARequest<T> : BaseYiDARequest<T>, IYiDARequest<T> where T : YiDAResponse
    {
        /// <summary>
        /// 当前页 范围 [1,∞]
        /// </summary>
        public int currentPage { get; private set; }
        /// <summary>
        /// 每页记录数 范围 [1,100]
        /// </summary>
        public int pageSize { get; private set; }
        /// <summary>
        /// 根据表单内组件值查询
        /// </summary>
        public string searchFieldJson { get; private set; }
        /// <summary>
        /// 根据数据提交人工号查询
        /// </summary>
        public string originatorId { get; private set; }
        /// <summary>
        /// createFrom和createTo两个时间构造一个时间段。查询在该时间段创建的数据列表。yyyy-MM-DD格式
        /// </summary>
        public string createFrom { get; private set; }
        /// <summary>
        /// createFrom和createTo两个时间构造一个时间段。查询在该时间段创建的数据列表。yyyy-MM-DD格式
        /// </summary>
        public string createTo { get; private set; }
        /// <summary>
        /// modifiedFrom和modifiedTo构成一个时间段，查询在该时间段有修改的数据列表。yyyy-MM-DD格式
        /// </summary>
        public string modifiedFrom { get; private set; }
        /// <summary>
        /// modifiedFrom和modifiedTo构成一个时间段，查询在该时间段有修改的数据列表。yyyy-MM-DD格式
        /// </summary>
        public string modifiedTo { get; private set; }

        /// <summary>
        /// 空构造
        /// </summary>
        public BasePageYiDARequest()
        {
            currentPage = 1;
            pageSize = 10;
        }

        /// <summary>
        /// 设置 当前页
        /// </summary>
        /// <param name="currentPage">范围 [1,∞]</param>
        public void SetCurrentPage(int currentPage) { this.currentPage = currentPage < 1 ? 1 : currentPage; }
        /// <summary>
        /// 设置 每页记录数
        /// </summary>
        /// <param name="pageSize">范围 [1,100]</param>
        public void SetPageSize(int pageSize) { this.pageSize = pageSize < 1 ? 1 : pageSize > 100 ? 100 : pageSize; }

        /// <summary>
        /// 设置 数据提交人工号
        /// </summary>
        /// <param name="originatorId"></param>
        public void SetOriginatorId(string originatorId) { this.originatorId = originatorId; }

        /// <summary>
        /// 根据表单内组件值查询
        /// </summary>
        /// <param name="obj"></param>
        public void SetSearchFieldJson(object obj) { this.searchFieldJson = obj.ToDictionaryOfObjectValue().Serialize(); }
        /// <summary>
        /// 根据表单内组件值查询
        /// </summary>
        /// <param name="dictionary"></param>
        public void SetSearchFieldJson(IDictionary<string, string> dictionary) { this.searchFieldJson = dictionary.Serialize(); }

        /// <summary>
        ///设置 时间段创建的数据列表 开始时间
        /// </summary>
        /// <param name="date"></param>
        public void SetCreateFrom(DateTime? date) { this.createFrom = date?.ToString("yyyy-MM-dd"); }
        /// <summary>
        ///设置 时间段创建的数据列表 开始时间。
        /// </summary>
        /// <param name="date">yyyy-MM-DD格式</param>
        public void SetCreateFrom(string dateStr) { this.createFrom = dateStr; }
        /// <summary>
        ///设置 时间段创建的数据列表 结束时间
        /// </summary>
        /// <param name="date"></param>
        public void SetCreateTo(DateTime? date) { this.createTo = date?.ToString("yyyy-MM-dd"); }
        /// <summary>
        ///设置 时间段创建的数据列表 结束时间。
        /// </summary>
        /// <param name="date">yyyy-MM-DD格式</param>
        public void SetCreateTo(string dateStr) { this.createTo = dateStr; }

        /// <summary>
        ///设置 时间段修改的数据列表 开始时间
        /// </summary>
        /// <param name="date"></param>
        public void SetModifiedFrom(DateTime? date) { this.modifiedFrom = date?.ToString("yyyy-MM-dd"); }
        /// <summary>
        ///设置 时间段修改的数据列表 开始时间
        /// </summary>
        /// <param name="date">yyyy-MM-DD格式</param>
        public void SetModifiedFrom(string dateStr) { this.modifiedFrom = dateStr; }
        /// <summary>
        ///设置 时间段修改的数据列表 结束时间
        /// </summary>
        /// <param name="date">结束时间</param>
        public void SetModifiedTo(DateTime? date) { this.modifiedTo = date?.ToString("yyyy-MM-dd"); }
        /// <summary>
        ///设置 时间段修改的数据列表 结束时间
        /// </summary>
        /// <param name="date">yyyy-MM-DD格式</param>
        public void SetModifiedTo(string dateStr) { this.modifiedTo = dateStr; }


    }
}
