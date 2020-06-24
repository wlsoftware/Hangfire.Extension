using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Hangfire.Extension
{
    public class AddRecurringJob : AddJobAbstract
    {
        public AddRecurringJob(Type jobClass) : base(jobClass) { }

        public override void Add(JobSetting jobSetting)
        {
            var (methodCall, x) = GetMethodCall(JobMethodInfo);
            var methodCallExpression = Expression.Call(
                typeof(RecurringJob),
                nameof(RecurringJob.AddOrUpdate),
                new Type[] { JobMethodInfo.DeclaringType },
                new Expression[]
                {
                    Expression.Constant(jobSetting.TypeName),
                    Expression.Lambda(methodCall, x),
                    Expression.Constant(jobSetting.Cron),
                    Expression.Constant(TimeZoneInfo.Utc),
                    Expression.Constant(jobSetting.Queue)
                });
            Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
        }
    }
}
