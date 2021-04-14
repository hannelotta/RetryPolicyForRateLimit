using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RateLimitedApi;

var host = CreateHostBuilder(args).Build();

await host.RunAsync();

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host
        .CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((hostContext, services) =>
        {

        })
        .ConfigureWebHost(hostBuilder =>
        {
            hostBuilder.UseStartup<Startup>().UseKestrel();
        });

    return hostBuilder;
}