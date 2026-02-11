using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Samsara.Infrastructure.Samsara.Client;
using Samsara.Infrastructure.Samsara.Options;

namespace Samsara.ETL.Extensions;

public static class ClientExtensions
{
    public static HostApplicationBuilder AddSamsaraClient(this HostApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

        builder.Services.AddOptions<SamsaraOptions>()
            .Bind(builder.Configuration.GetSection("Samsara"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddOptions<SensorHistoryOptions>()
            .Bind(builder.Configuration.GetSection("SensorHistory"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddHttpClient<ISamsaraClient, SamsaraClient>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<SamsaraOptions>>().Value;
            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.ApiKey}");
        }).AddStandardResilienceHandler(); // this is an extension method that adds Polly-based retry and circuit breaker policies Link: https://learn.microsoft.com/en-us/dotnet/core/resilience/http-resilience?tabs=dotnet-cli

        return builder;
    }
}