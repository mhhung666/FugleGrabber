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
    private readonly string _quoteApiUrl;
    
    private readonly string[] _types = { "EQUITY", "INDEX", "WARRANT", "ODDLOT" };
    private readonly string[] _exchanges = { "TWSE", "TPEx" };
    private readonly string[] _markets = { "TSE", "OTC", "ESB", "TIB", "PSB" };
    
    public ApiBackgroundService(ILogger<ApiBackgroundService> logger, IConfiguration configuration, FugleDb fugleDb)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _apiKey = configuration["ApiSettings:ApiKey"]!;
        _tickersApiUrl = configuration["ApiSettings:TickersApi"]!;
        _tickerApiUrl = configuration["ApiSettings:TickerApi"]!;
        _quoteApiUrl = configuration["ApiSettings:QuoteApi"]!;
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
                // await UpdateTickers();
                // await UpdateTickerInfoAsync();
                await InsertUpdateStockQuote();
                
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
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing other functionality.");
            }
        }
    }

    /// <summary>
    /// 取得股票或指數列表
    /// </summary>
    private async Task UpdateTickers()
    {
        var tickers = await GetTickersAsync();
        // Insert Update Ticker
        await _fugleDb.InsertUpdateTickersAsync(tickers);
    }

    /// <summary>
    /// 取得股票基本資料
    /// </summary>
    private async Task UpdateTickerInfoAsync()
    {
        // Get Tickers List from Db
        var tickerList = await _fugleDb.GetTickerByTypeAsync("EQUITY");
                
        // Insert Update Ticker
        foreach (var ticker in tickerList)
        {
            var tickerInfo = await GetTickerInfoAsync(ticker.Symbol);
            await _fugleDb.InsertUpdateTickerInfoAsync(tickerInfo);
        }
    }

    /// <summary>
    /// 取得股票即時報價, 目前只打算取盤後時間資料
    /// </summary>
    private async Task InsertUpdateStockQuote()
    {
        // Get Tickers List from Db
        var tickerList = await _fugleDb.GetTickerByTypeAsync("EQUITY");
        
        // Insert Update Ticker
        foreach (var ticker in tickerList)
        {
            if (ticker.Name != string.Empty)
            {
                var stockQuoteInfo = await GetQuoteInfoAsync(ticker.Symbol);
                await InsertOrUpdateStockQuoteAsync(stockQuoteInfo);
            }
        }
    }
}