using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Samsara.ETL.Extensions;
using Samsara.ETL.Features.GatewaySync;
using Samsara.ETL.Features.SensorSync;
using Samsara.ETL.Features.TrailerSync;

var builder = Host.CreateApplicationBuilder(args);

builder.AddSamsaraClient(options =>
{
    builder.Configuration.GetSection("Samsara").Bind(options);
});

builder.Services.AddServices();

var host = builder.Build();

// Run the job once
using (var scope = host.Services.CreateScope())
{
    var job = scope.ServiceProvider.GetRequiredService<SensorSyncJob>();
    await job.ExecuteAsync();
    var job1 = scope.ServiceProvider.GetRequiredService<TrailerSyncJob>();
    var job2 = scope.ServiceProvider.GetRequiredService<SensorSyncJob>();
    var job3 = scope.ServiceProvider.GetRequiredService<GatewaySyncJob>();

    await job1.ExecuteAsync();
    await job2.ExecuteAsync();
    await job3.ExecuteAsync();

}