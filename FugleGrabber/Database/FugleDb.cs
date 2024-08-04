using System.Data;
using Dapper;
using FugleGrabber.Model;

namespace FugleGrabber.Database;

public class FugleDb
{
    private readonly DatabaseHelper _databaseHelper;

    public FugleDb(DatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

    #region Tickers Table

    public async Task<IEnumerable<Ticker>> GetAllTickersAsync()
    {
        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            var query = "SELECT * FROM tickers";
            return await connection.QueryAsync<Ticker>(query);
        }
    }

    public async Task<Ticker> GetTickerBySymbolAsync(string symbol)
    {
        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            var query = "SELECT * FROM tickers WHERE symbol = @Symbol";
            return await connection.QuerySingleOrDefaultAsync<Ticker>(query, new { Symbol = symbol });
        }
    }

    public async Task<List<Ticker>> GetTickerByTypeAsync(string type)
    {
        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            var query = "SELECT * FROM tickers WHERE type = @Type";
            var result = await connection.QueryAsync<Ticker>(query, new { Type = type });
            return result.ToList();
        }
    }

    public async Task InsertUpdateTickersAsync(IEnumerable<Ticker> tickers)
    {
        var query = @"
            INSERT INTO tickers (symbol, name, type, exchange, market, update_time) 
            VALUES (@Symbol, @Name, @Type, @Exchange, @Market, @UpdateTime)
            ON DUPLICATE KEY UPDATE
                name = VALUES(name),
                type = VALUES(type),
                exchange = VALUES(exchange),
                market = VALUES(market),
                update_time = VALUES(update_time);";

        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            await connection.ExecuteAsync(query, tickers);
        }
    }

    #endregion

    #region Ticker

    public async Task InsertUpdateTickerInfoAsync(TickerInfo tickerInfo)
    {
        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            var query = @"
            INSERT INTO tickerInfo (
                symbol, name, type, exchange, market, industry, security_type, 
                previous_close, reference_price, limit_up_price, limit_down_price, 
                can_day_trade, can_buy_day_trade, can_below_flat_margin_short_sell, 
                can_below_flat_sbl_short_sell, is_attention, is_disposition, 
                is_unusually_recommended, is_specific_abnormally, matching_interval, 
                security_status, board_lot, trading_currency, update_time
            ) VALUES (
                @Symbol, @Name, @Type, @Exchange, @Market, @Industry, @SecurityType, 
                @PreviousClose, @ReferencePrice, @LimitUpPrice, @LimitDownPrice, 
                @CanDayTrade, @CanBuyDayTrade, @CanBelowFlatMarginShortSell, 
                @CanBelowFlatSblShortSell, @IsAttention, @IsDisposition, 
                @IsUnusuallyRecommended, @IsSpecificAbnormally, @MatchingInterval, 
                @SecurityStatus, @BoardLot, @TradingCurrency, @UpdateTime
            ) ON DUPLICATE KEY UPDATE
                name = VALUES(name),
                type = VALUES(type),
                exchange = VALUES(exchange),
                market = VALUES(market),
                industry = VALUES(industry),
                security_type = VALUES(security_type),
                previous_close = VALUES(previous_close),
                reference_price = VALUES(reference_price),
                limit_up_price = VALUES(limit_up_price),
                limit_down_price = VALUES(limit_down_price),
                can_day_trade = VALUES(can_day_trade),
                can_buy_day_trade = VALUES(can_buy_day_trade),
                can_below_flat_margin_short_sell = VALUES(can_below_flat_margin_short_sell),
                can_below_flat_sbl_short_sell = VALUES(can_below_flat_sbl_short_sell),
                is_attention = VALUES(is_attention),
                is_disposition = VALUES(is_disposition),
                is_unusually_recommended = VALUES(is_unusually_recommended),
                is_specific_abnormally = VALUES(is_specific_abnormally),
                matching_interval = VALUES(matching_interval),
                security_status = VALUES(security_status),
                board_lot = VALUES(board_lot),
                trading_currency = VALUES(trading_currency),
                update_time = VALUES(update_time)";

            await connection.ExecuteAsync(query, tickerInfo);
        }
    }

    #endregion

    #region Stock Quote

    public async Task InsertOrUpdateStockQuoteAsync(StockQuote stockQuote)
    {
        using (IDbConnection connection = _databaseHelper.CreateConnection())
        {
            var query = @"
            INSERT INTO stockQuote (
                symbol, referencePrice, previousClose, openPrice, openTime, 
                highPrice, highTime, lowPrice, lowTime, closePrice, 
                closeTime, avgPrice, priceChange, changePercent, amplitude, 
                lastPrice, lastSize, isClose, lastUpdated
            ) VALUES (
                @Symbol, @ReferencePrice, @PreviousClose, @OpenPrice, @OpenTime, 
                @HighPrice, @HighTime, @LowPrice, @LowTime, @ClosePrice, 
                @CloseTime, @AvgPrice, @PriceChange, @ChangePercent, @Amplitude, 
                @LastPrice, @LastSize, @IsClose, @LastUpdated
            ) ON DUPLICATE KEY UPDATE 
                referencePrice = @ReferencePrice, 
                previousClose = @PreviousClose, 
                openPrice = @OpenPrice, 
                openTime = @OpenTime, 
                highPrice = @HighPrice, 
                highTime = @HighTime, 
                lowPrice = @LowPrice, 
                lowTime = @LowTime, 
                closePrice = @ClosePrice, 
                closeTime = @CloseTime, 
                avgPrice = @AvgPrice, 
                priceChange = @PriceChange, 
                changePercent = @ChangePercent, 
                amplitude = @Amplitude, 
                lastPrice = @LastPrice, 
                lastSize = @LastSize, 
                isClose = @IsClose, 
                lastUpdated = @LastUpdated;
        ";

            await connection.ExecuteAsync(query, stockQuote);
        }
    }

    #endregion
}