using FugleGrabber.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FugleGrabber.Service;

public class ApiBackgroundService : BackgroundService
{
    private readonly ILogger<ApiBackgroundService> _logger;
    private readonly HttpClient _httpClient;
    private readonly TickerService _tickerService;
    private readonly string _apiKey;
    private readonly string _apiUrl;
    
    public ApiBackgroundService(ILogger<ApiBackgroundService> logger, IConfiguration configuration, TickerService tickerService)
    {
        _logger = logger;
        _httpClient = new HttpClient();
        _apiKey = configuration["ApiSettings:ApiKey"];
        _apiUrl = configuration["ApiSettings:ApiUrl"];
        _tickerService = tickerService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl);
                request.Headers.Add("X-API-KEY", _apiKey);

                var response = await _httpClient.SendAsync(request, stoppingToken);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"API Response: {content}");
                
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
                var tickers = apiResponse.Data.Select(d => new Ticker
                {
                    Symbol = d.Symbol,
                    Name = d.Name ?? string.Empty,
                    Type = apiResponse.Type,
                    Exchange = apiResponse.Exchange,
                    UpdateTime = DateTime.Now
                });

                await _tickerService.InsertOrUpdateTickersAsync(tickers);
                
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calling the API.");
            }
        }
    }
}