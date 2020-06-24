using Hangfire.Server;

namespace Hangfire.Extension
{
    public interface IJobDependency
    {
        void Execute(PerformContext context);
    }
}
