using Hangfire;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Hangfire.Extension
{
    public class AddOneTimeJob : AddJobAbstract
    {
        public AddOneTimeJob(Type jobClass) : base(jobClass) { }

        public override void Add(JobSetting jobSetting)
        {
            var (methodCall, x) = GetMethodCall(JobMethodInfo);
            var methodCallExpression = Expression.Call(
                typeof(BackgroundJob),
                nameof(BackgroundJob.Enqueue),
                new Type[] { JobMethodInfo.DeclaringType },
                new Expression[]
                {
                    Expression.Lambda(methodCall, x),
                });
            Expression.Lambda(methodCallExpression).Compile().DynamicInvoke();
        }
    }
}
