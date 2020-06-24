using System;
using System.Collections.Generic;
using System.Text;
using Hangfire.Server;

namespace Hangfire.Extension.Example.Jobs
{
    /// <summary>
    /// 执行一次性任务示例
    /// </summary>
    public class MyOneTimeJob : IJobDependency
    {
        public void Execute(PerformContext context)
        {
            Console.WriteLine($"{DateTime.Now}-execute MyOneTimeJob");
        }
    }
}
