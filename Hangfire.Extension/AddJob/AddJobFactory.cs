using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Extension
{
    public class AddJobFactory
    {
        public static AddJobAbstract Get(JobRunType runType, Type jobClass)
        {
            switch (runType)
            {
                case JobRunType.Recurring:
                    return new AddRecurringJob(jobClass);
                case JobRunType.OneTime:
                    return new AddOneTimeJob(jobClass);
                case JobRunType.Delayed:
                    return new AddDelayJob(jobClass);
                default:
                    throw new Exception($"unkown runType {runType}");
            }
        }
    }
}
