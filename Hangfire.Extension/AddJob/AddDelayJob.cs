using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Hangfire.Extension
{
    public class AddDelayJob : AddJobAbstract
    {
        public AddDelayJob(Type jobClass) : base(jobClass) { }

        public override void Add(JobSetting jobSetting)
        {
            var (methodCall, x) = GetMethodCall(JobMethodInfo);
            var methodCallExpression = Expression.Call(
                typeof(BackgroundJob),
                nameof(BackgroundJob.Schedule),
                new Type[] { JobMethodInfo.DeclaringType },
                new Expression[]
                {
                    Expression.Lambda(methodCall, x),
                    Expression.Constant(TimeSpan.FromSeconds(jobSetting.DelaySeconds.Value)),
                });
            Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
        }
    }
}
