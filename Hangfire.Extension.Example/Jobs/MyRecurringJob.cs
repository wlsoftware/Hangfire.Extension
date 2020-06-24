using System;
using System.Collections.Generic;
using System.Text;
using Hangfire.Server;

namespace Hangfire.Extension.Example.Jobs
{
    /// <summary>
    /// 循环任务示例
    /// </summary>
    public class MyRecurringJob : IJobDependency
    {
        public void Execute(PerformContext context)
        {
            var paras = context.GetJobData();
            Console.WriteLine($"{DateTime.Now}-execute MyRecurringJob-param1:{paras["param1"]}");
        }
    }
}
