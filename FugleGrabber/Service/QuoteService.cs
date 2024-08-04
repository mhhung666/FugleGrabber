using FugleGrabber.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FugleGrabber.Service;

public partial class ApiBackgroundService : BackgroundService
{
    private async Task<StockQuoteDto> GetQuoteInfoAsync(string symbol)
    {
        try
        {
            var requestUrl = $"{_quoteApiUrl}/{symbol}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("X-API-KEY", _apiKey);
        
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"API Response: {content}");
        
            // fugle 免費版 一分鐘60次
            await Task.Delay(1050);
        
            var stockQuoteInfo = JsonConvert.DeserializeObject<StockQuoteDto>(content);

            return stockQuoteInfo ?? new StockQuoteDto();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task InsertOrUpdateStockQuoteAsync(StockQuoteDto stockQuoteDto)
    {
        // Map the stockQuoteInfo to StockQuote object
        var stockQuote = new StockQuote
        {
            Symbol = stockQuoteDto.Symbol,
            ReferencePrice = stockQuoteDto.ReferencePrice,
            PreviousClose = stockQuoteDto.PreviousClose,
            OpenPrice = stockQuoteDto.OpenPrice,
            OpenTime = stockQuoteDto.OpenTime,
            HighPrice = stockQuoteDto.HighPrice,
            HighTime = stockQuoteDto.HighTime,
            LowPrice = stockQuoteDto.LowPrice,
            LowTime = stockQuoteDto.LowTime,
            ClosePrice = stockQuoteDto.ClosePrice,
            CloseTime = stockQuoteDto.CloseTime,
            AvgPrice = stockQuoteDto.AvgPrice,
            PriceChange = stockQuoteDto.Change,
            ChangePercent = stockQuoteDto.ChangePercent,
            Amplitude = stockQuoteDto.Amplitude,
            LastPrice = stockQuoteDto.LastPrice,
            LastSize = stockQuoteDto.LastSize,
            IsClose = stockQuoteDto.IsClose,
            LastUpdated = stockQuoteDto.LastUpdated
        };
        
        // Insert or update the stock quote in the database
        await _fugleDb.InsertOrUpdateStockQuoteAsync(stockQuote);
    }
}