using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Samsara.Infrastructure.Client;
using Samsara.Infrastructure.Options;

namespace Samsara.ETL.Extensions;

public static class ClientExtensions
{
    public static HostApplicationBuilder AddSamsaraClient(
        this HostApplicationBuilder builder,
        Action<SamsaraOptions> configureOptions)
    {
        builder.Services.Configure(configureOptions);

        builder.Services.AddHttpClient<ISamsaraClient, SamsaraClient>((serviceProvider, httpClient) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<SamsaraOptions>>().Value;
            httpClient.BaseAddress = new Uri(options.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {options.ApiKey}");
        });

        return builder;
    }
}