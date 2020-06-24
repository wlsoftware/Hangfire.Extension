using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Hangfire.Extension
{
    public class JobHelper
    {
        public static JobSettings Settings { get; private set; }

        public static JobSettings InitSettings(IConfiguration configuration)
        {
            var section = configuration.GetSection("jobs");
            if (section != null)
            {
                var config = section.Get<List<JobSetting>>();
                if (config != null)
                    Settings = new JobSettings() { Jobs = config };
            }
            return Settings;
        }
        
        public static void InitJobs()
        {
            var baseType = typeof(IJobDependency);
            if (Settings != null && Settings.Jobs != null && Settings.Jobs.Count > 0)
            {
                var assemblies = GetReferenceAssembly();
                var jobClasss = assemblies
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJobDependency))))
                .ToList();
                foreach (var j in Settings.Jobs)
                {
                    if (!j.Enabled.HasValue || !j.Enabled.Value)
                        continue;
                    var jc = jobClasss.Where(r => r.Name == j.Name).FirstOrDefault();
                    if (jc == null)
                        continue;
                    j.TypeName = jc.FullName;
                    InitJobRunType(j);
                    if (j.RunType == JobRunType.OneTime || j.RunType == JobRunType.Delayed)
                        j.Queue = "default";
                    if (j.RunType == JobRunType.Recurring && (string.IsNullOrWhiteSpace(j.Cron)|| string.IsNullOrWhiteSpace(j.Queue)))
                    {
                        continue;
                    }
                    AddJobFactory.Get(j.RunType,jc).Add(j);
                }
            }
        }
        
        public static List<Assembly> GetReferenceAssembly()
        {
            var result = _referenceAssembly;
            if (!result.Any())
            {
                var assembliesNames = GetAssembliesName();
                var pattern = $"{string.Join("|", defaultNotRelated.Select(z => z + ".\\w*"))}";
                Regex notRelatedRegex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);

                assembliesNames.Where(a => !notRelatedRegex.IsMatch(a.FullName))
                    .ToList().ForEach(z =>
                    {
                        _referenceAssembly.Add(Assembly.Load(z));
                    });
                result = _referenceAssembly;
            }
            return result;
        }

        #region private

        private static List<Assembly> _referenceAssembly = new List<Assembly>();

        private readonly static List<string> defaultNotRelated = new List<string> {
            "Microsoft",
            "System",
            "netstandard",
            "Newtonsoft"
        };

        private static List<AssemblyName> GetAssembliesName()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(z => z.GetName()).ToList();
            var assembliesNames = Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList();
            return assembliesNames.Where(r => !loadedAssemblies.Exists(z => z.FullName == r.FullName))
                .Concat(loadedAssemblies).ToList();
        }

        private static void InitJobRunType(JobSetting setting)
        {
            if (setting.RunType == JobRunType.Unkown)
            {
                if (!string.IsNullOrWhiteSpace(setting.Cron))
                {
                    setting.RunType = JobRunType.Recurring;
                    return;
                }
                if (setting.DelaySeconds.HasValue && setting.DelaySeconds > 0)
                {
                    setting.RunType = JobRunType.Delayed;
                    return;
                }
                setting.RunType = JobRunType.OneTime;
            }
        }

        #endregion
    }
}
