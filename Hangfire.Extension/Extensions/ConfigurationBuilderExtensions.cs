using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.Extension
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddJobConfigFile(this IConfigurationBuilder builder, string path = "jobs.json")
        {
            return builder.AddJsonFile(path);
        }
    }
}
