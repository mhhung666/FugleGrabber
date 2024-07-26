using FugleGrabber.Database;
using FugleGrabber.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FugleGrabber.Service;

public partial class ApiBackgroundService : BackgroundService
{
    private readonly ILogger<ApiBackgroundService> _logger;
    private readonly HttpClient _httpClient;
    private readonly FugleDb _fugleDb;
    private readonly string _apiKey;
    private readonly string _tickersApiUrl;
    private readonly string _tickerApiUrl;
    
    private readonly string[] _types = { "EQUITY", "INDEX", "WARRANT", "ODDLOT" };
    private readonly string[] _exchanges = { "TWSE", "TPEx" };
    private readonly string[] _markets = { "TSE", "OTC", "ESB", "TIB", "PSB" };
    
    public ApiBackgroundService(ILogger<ApiBackgroundService> logger, IConfiguration configuration, FugleDb fugleDb)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _apiKey = configuration["ApiSettings:ApiKey"];
        _tickersApiUrl = configuration["ApiSettings:TickersApi"];
        _tickerApiUrl = configuration["ApiSettings:TickerApi"];
        _fugleDb = fugleDb;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var dailyTask = Task.Run(async () => await DailyTask(stoppingToken), stoppingToken);
        var otherTask = Task.Run(async () => await OtherTask(stoppingToken), stoppingToken);

        await Task.WhenAll(dailyTask, otherTask);
    }
    
    private async Task DailyTask(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // var tickers = await GetTickersAsync();
                // // Insert Update Ticker
                // await _fugleDb.InsertUpdateTickersAsync(tickers);
                
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling the API.");
            }
        }
    }

    private async Task OtherTask(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // do ticker update
                var tickerList = await _fugleDb.GetTickerByTypeAsync("EQUITY");
                
                // Insert Update Ticker
                foreach (var ticker in tickerList)
                {
                    var tickerInfo = await GetTickerInfoAsync(ticker.Symbol);
                    await _fugleDb.InsertUpdateTickerInfoAsync(tickerInfo);
                }

                // 每五分钟一次
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing other functionality.");
            }
        }
    }
}