using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Hangfire.Extension
{
    public abstract class AddJobAbstract
    {
        protected Type JobClass;

        public AddJobAbstract(Type jobClass)
        {
            JobClass = jobClass;
        }

        public abstract void Add(JobSetting jobSetting);

        protected (MethodCallExpression, ParameterExpression) GetMethodCall(MethodInfo method)
        {
            var parameters = method.GetParameters();
            Expression[] args = new Expression[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = Expression.Default(parameters[i].ParameterType);
            }

            var x = Expression.Parameter(method.DeclaringType, "x");
            var methodCall = Expression.Call(x, method, args);
            return (methodCall, x);
        }

        protected MethodInfo JobMethodInfo
        {
            get
            {
                if (JobClass != null)
                {
                    return JobClass.GetTypeInfo().GetDeclaredMethod(nameof(IJobDependency.Execute));
                }
                return null;
            }
        }
    }
}
