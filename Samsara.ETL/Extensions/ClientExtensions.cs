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
        builder.Services.AddOptions<SamsaraOptions>()
          .Bind(builder.Configuration.GetSection("Samsara"))
          .ValidateDataAnnotations()
          .ValidateOnStart();

        builder.Services.AddOptions<SensorHistoryOptions>()
          .Bind(builder.Configuration.GetSection("SensorHistory"));

        builder.Services.AddHttpClient<ISamsaraClient, SamsaraClient>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<SamsaraOptions>>().Value;
            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.ApiKey}");
        });

        return builder;
    }
}