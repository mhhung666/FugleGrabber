using FugleGrabber.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FugleGrabber.Service;

public partial class ApiBackgroundService : BackgroundService
{
    private async Task<IEnumerable<Ticker>> GetTickersAsync()
    {
        foreach (var type in _types)
        {
            foreach (var exchange in _exchanges)
            {
                foreach (var market in _markets)
                {
                    var requestUrl = $"{_tickersApiUrl}?type={type}&exchange={exchange}&market={market}";
                    var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                    request.Headers.Add("X-API-KEY", _apiKey);

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"API Response: {content}");

                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
                    var tickers = apiResponse.Data.Select(d => new Ticker
                    {
                        Symbol = d.Symbol,
                        Name = d.Name ?? string.Empty,
                        Type = type,
                        Exchange = exchange,
                        Market = market,
                        UpdateTime = DateTime.Now
                    });

                    return tickers;
                }
            }
        }

        return new List<Ticker>();
    }

    private async Task<TickerInfo> GetTickerInfoAsync(string symbol)
    {
        var requestUrl = $"{_tickerApiUrl}/{symbol}";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        request.Headers.Add("X-API-KEY", _apiKey);
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"API Response: {content}");

        // fugle 免費版 一分鐘60次
        await Task.Delay(1050);

        var tickerInfo = JsonConvert.DeserializeObject<TickerInfo>(content);
        tickerInfo.UpdateTime = DateTime.Now;

        return tickerInfo ?? new TickerInfo();
    }

}