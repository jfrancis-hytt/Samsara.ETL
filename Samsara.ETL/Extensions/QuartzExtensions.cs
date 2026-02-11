using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Samsara.ETL.Pipelines.Gateway;
using Samsara.ETL.Pipelines.Sensor;
using Samsara.ETL.Pipelines.SensorHistory;
using Samsara.ETL.Pipelines.SensorTemperature;
using Samsara.ETL.Pipelines.Trailer;

namespace Samsara.ETL.Extensions;

public static class QuartzExtensions
{
    public static void AddQuartzJobs(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            // Link: https://www.quartz-scheduler.org/documentation/quartz-2.3.0/tutorials/crontrigger.html
            q.AddJobAndTrigger<TrailerJob>("trailer-sync", "0 0/30 * * * ?");
            q.AddJobAndTrigger<GatewayJob>("gateway-sync", "0 0/30 * * * ?");
            q.AddJobAndTrigger<SensorJob>("sensor-sync", "0 0/30 * * * ?");
            q.AddJobAndTrigger<SensorTemperatureJob>("sensor-temperature-sync", "0 0/15 * * * ?");
            q.AddJobAndTrigger<SensorHistoryJob>("sensor-history-sync", "0 0/15 * * * ?");
        });

        services.AddQuartzHostedService(o => o.WaitForJobsToComplete = true);
    }

    private static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator q, string identity, string cronExpression)
        where T : IJob
    {
        var jobKey = new JobKey(identity);

        q.AddJob<T>(j => j.WithIdentity(jobKey));
        q.AddTrigger(t => t
            .ForJob(jobKey)
            .WithIdentity($"{identity}-trigger")
            .WithCronSchedule(cronExpression));
    }
}
