using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;

namespace Hangfire.Extension.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJobConfigFile()//add job config file
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                Configuration = builder.Build();

                var services = new ServiceCollection();
                services.RegisterJobs();//register jobs

                JobHelper.InitSettings(Configuration);
                var server = InitHangfireServer();
                JobHelper.InitJobs();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadKey();
        }

        static IConfiguration Configuration;

        private static BackgroundJobServer InitHangfireServer()
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new Hangfire.SqlServer.SqlServerStorageOptions()
            {
                QueuePollInterval = TimeSpan.FromSeconds(1)
            });

            var serverOp = new BackgroundJobServerOptions()
            {
                WorkerCount = Environment.ProcessorCount * 20,
                SchedulePollingInterval = TimeSpan.FromSeconds(1)
            };
            var jobs = JobHelper.Settings?.Jobs;
            if (jobs != null && jobs.Count > 0)
            {
                if (jobs.Exists(a => a.Enabled == true &&!string.IsNullOrWhiteSpace(a.Queue)))
                    serverOp.Queues = jobs.Where(a => a.Enabled == true).Select(b => b.Queue).Distinct().ToArray();
            }
            var server = new BackgroundJobServer(serverOp);
            return server;
        }
    }
}
