using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Extension.Example.Jobs
{
    /// <summary>
    /// 延迟执行任务示例
    /// </summary>
    public class MyDelayJob : IJobDependency
    {
        public void Execute(PerformContext context)
        {
            Console.WriteLine($"{DateTime.Now}-execute MyDelayJob");
        }
    }
}
