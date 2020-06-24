using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangfire.Extension
{
    /// <summary>
    /// 任务设置wrap
    /// </summary>
    public class JobSettings
    {
        public List<JobSetting> Jobs { get; set; }
    }
    /// <summary>
    /// 任务设置
    /// </summary>
    public class JobSetting
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Queue { get; set; }
        public List<JobSettingData> Datas { get; set; }
        /// <summary>
        /// Cron表达式 优先
        /// </summary>
        public string Cron { get; set; }

        public int? DelaySeconds { get; set; }

        public bool? Enabled { get; set; } = true;

        public JobRunType RunType { get; set; }

        public string TypeName { get; set; }
    }
}
