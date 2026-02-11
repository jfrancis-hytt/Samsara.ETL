using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Samsara.ETL.Extensions;
using Samsara.ETL.Pipelines.Gateway;
using Samsara.ETL.Pipelines.Sensor;
using Samsara.ETL.Pipelines.SensorTemperature;
using Samsara.ETL.Pipelines.SensorHistory;
using Samsara.ETL.Pipelines.Trailer;
using Microsoft.Extensions.Configuration;

var builder = Host.CreateApplicationBuilder(args);

// DEBUG: dump all config keys containing "sensor" (case-insensitive)
foreach (var kvp in builder.Configuration.AsEnumerable())
{
    if (kvp.Key.Contains("Sensor", StringComparison.OrdinalIgnoreCase) || kvp.Key.Contains("History", StringComparison.OrdinalIgnoreCase))
        Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
}

builder.AddSamsaraClient();

builder.Services.AddServices();
builder.Services.AddRepositories();

var host = builder.Build();

var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
var ct = lifetime.ApplicationStopping;

// Test Jobs
using (var scope = host.Services.CreateScope())
{
    var job1 = scope.ServiceProvider.GetRequiredService<TrailerJob>();
    var job2 = scope.ServiceProvider.GetRequiredService<SensorJob>();
    var job3 = scope.ServiceProvider.GetRequiredService<GatewayJob>();
    var job4 = scope.ServiceProvider.GetRequiredService<SensorTemperatureJob>();
    var job5 = scope.ServiceProvider.GetRequiredService<SensorHistoryJob>();

    // Run parent table jobs first (can run in parallel)
    //await Task.WhenAll(
    //    job1.ExecuteAsync(ct),
    //    job2.ExecuteAsync(ct),
    //    job3.ExecuteAsync(ct)
    //);

    // Run dependent jobs after parents are populated
    await Task.WhenAll(
        //job4.ExecuteAsync(ct),
        job5.ExecuteAsync(ct)
    );

}