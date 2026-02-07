using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Samsara.ETL.Extensions;
using Samsara.ETL.Features.SensorSync;

var builder = Host.CreateApplicationBuilder(args);

builder.AddSamsaraClient(options =>
{
    options.BaseUrl = "https://api.samsara.com";
    options.ApiKey = "";
});

builder.Services.AddServices();

// TODO: Add Config from Environment Variables 
//builder.AddSamsaraClient(options =>
//{
//    builder.Configuration.GetSection("Samsara").Bind(options);
//});

var host = builder.Build();

// Run the job once
using (var scope = host.Services.CreateScope())
{
    var job = scope.ServiceProvider.GetRequiredService<SensorSyncJob>();
    await job.ExecuteAsync();
}

Console.WriteLine("Job completed. Press any key to exit...");
Console.ReadKey();