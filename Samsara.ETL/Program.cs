using Microsoft.Extensions.Hosting;
using Samsara.ETL.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.AddSamsaraClient();

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddQuartzJobs();

var host = builder.Build();

host.Run();
