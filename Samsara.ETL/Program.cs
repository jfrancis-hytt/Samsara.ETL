using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Samsara.ETL.Extensions;
using Samsara.ETL.Features.GatewaySync;
using Samsara.ETL.Features.SensorSync;
using Samsara.ETL.Features.SensorTemperatureSync;
using Samsara.ETL.Features.TrailerSync;

var builder = Host.CreateApplicationBuilder(args);

builder.AddSamsaraClient();

builder.Services.AddServices();

var host = builder.Build();

var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
var ct = lifetime.ApplicationStopping;

// Test Jobs
using (var scope = host.Services.CreateScope())
{
    var job1 = scope.ServiceProvider.GetRequiredService<TrailerSyncJob>();
    var job2 = scope.ServiceProvider.GetRequiredService<SensorSyncJob>();
    var job3 = scope.ServiceProvider.GetRequiredService<GatewaySyncJob>();
    var job4 = scope.ServiceProvider.GetRequiredService<SensorTemperatureSyncJob>();

    await Task.WhenAll(
        job1.ExecuteAsync(ct),
        job2.ExecuteAsync(ct),
        job3.ExecuteAsync(ct),
        job4.ExecuteAsync(ct)
    );

}