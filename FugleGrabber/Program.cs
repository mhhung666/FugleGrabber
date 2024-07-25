using System.Data;
using FugleGrabber.Database;
using FugleGrabber.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddSingleton(new DatabaseHelper(connectionString));
        services.AddTransient<IDbConnection>(_ => new MySqlConnection(connectionString));
        services.AddTransient<TickerService>();

        // 註冊背景服務
        services.AddHostedService<ApiBackgroundService>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();