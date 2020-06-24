using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Hangfire.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterJobs(this IServiceCollection services)
        {
            var referenceAssemblies = JobHelper.GetReferenceAssembly();
            var baseType = typeof(IJobDependency);
            referenceAssemblies?.ForEach(am => {
                foreach (var t in am.DefinedTypes.Select(z => z.AsType()))
                {
                    if (baseType.IsAssignableFrom(t) && t != baseType)
                        services.AddTransient(t);
                }
            });
            return services;
        }


    }
}
