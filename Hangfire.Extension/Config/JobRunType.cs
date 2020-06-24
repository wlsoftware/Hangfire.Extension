using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Extension
{
    /// <summary>
    /// 任务执行类型
    /// </summary>
    public enum JobRunType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unkown = 0,
        /// <summary>
        /// 循环任务
        /// </summary>
        Recurring = 1,
        /// <summary>
        /// 执行一次性任务
        /// </summary>
        OneTime = 2,
        /// <summary>
        /// 延时执行一次性任务
        /// </summary>
        Delayed = 3
    }
}
