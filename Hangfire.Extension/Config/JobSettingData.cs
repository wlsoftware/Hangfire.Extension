using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Extension
{
    /// <summary>
    /// 任务配置数据
    /// </summary>
    public class JobSettingData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
