using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Samsara.ETL.Extensions;
using Samsara.ETL.Pipelines.Gateway;
using Samsara.ETL.Pipelines.Sensor;
using Samsara.ETL.Pipelines.SensorTemperature;
using Samsara.ETL.Pipelines.SensorHistory;
using Samsara.ETL.Pipelines.Trailer;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.AddSamsaraClient();

builder.Services.AddServices();
builder.Services.AddRepositories();



var host = builder.Build();

var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();
var ct = lifetime.ApplicationStopping;

var logger = host.Services.GetRequiredService<ILogger<Program>>();

await host.StartAsync(); // Start the host

try
{
    using var scope = host.Services.CreateScope();

    var job1 = scope.ServiceProvider.GetRequiredService<TrailerJob>();
    var job2 = scope.ServiceProvider.GetRequiredService<SensorJob>();
    var job3 = scope.ServiceProvider.GetRequiredService<GatewayJob>();
    var job4 = scope.ServiceProvider.GetRequiredService<SensorTemperatureJob>();
    var job5 = scope.ServiceProvider.GetRequiredService<SensorHistoryJob>();

    // Run parent jobs first
    await Task.WhenAll(
        job1.ExecuteAsync(ct),
        job2.ExecuteAsync(ct),
        job3.ExecuteAsync(ct)
    );

    // Run dependent jobs after parents run
    await Task.WhenAll(
        job4.ExecuteAsync(ct),
        job5.ExecuteAsync(ct)
    );
}
catch (Exception ex)
{
    logger.LogCritical(ex, "ETL pipeline failed");
    Environment.ExitCode = 1; // Set exit code to indicate failure
}
finally
{
    await host.StopAsync(); // Ensure graceful shutdown
}