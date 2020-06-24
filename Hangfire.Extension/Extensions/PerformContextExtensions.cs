using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Hangfire.Extension
{
    public static class PerformContextExtensions
    {
        public static JobSetting GetJobSetting(this PerformContext context)
        {
            var jobTypeName = context?.BackgroundJob?.Job?.Type?.FullName;
            if (!string.IsNullOrWhiteSpace(jobTypeName))
            {
                return JobHelper.Settings.Jobs.FirstOrDefault(z => z.TypeName == jobTypeName);
            }
            return null;
        }

        public static IDictionary<string, string> GetJobData(this PerformContext context)
        {
            var res = new Dictionary<string, string>();
            var jobTypeName = context?.BackgroundJob?.Job?.Type?.FullName;
            if (!string.IsNullOrWhiteSpace(jobTypeName))
            {
                JobHelper.Settings.Jobs.FirstOrDefault(z => z.TypeName == jobTypeName).Datas.ForEach(z => {
                    if (!res.ContainsKey(z.Key))
                        res.Add(z.Key, z.Value);
                });
            }
            return res;
        }
    }
}
